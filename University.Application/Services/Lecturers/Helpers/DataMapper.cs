using University.Data.Data.Entities;
using University.Domain.Models;

namespace University.Application.Services.Lecturers.Helpers;

public static class DataMapper
{
    public static IQueryable<LecturerGetDto> MapDataToLecturerGetDto(this IQueryable<Lecturer> lecturers)
    {
        return lecturers.Select(lecturer => new LecturerGetDto
        {
            Id = lecturer.Id,
            Name = lecturer.Name,
            SurName = lecturer.SurName,
            Age = lecturer.Age,
            Users = lecturer.UsersLecturers
                .Where(ul => ul.User != null)
                .Select(l => new UserOnlyDto
                {
                    Id = l.User!.Id,
                    Name = l.User.Name,
                    SurName = l.User.SurName,
                    Age = l.User.Age
                }).ToList(),
            Courses = lecturer.CoursesLecturers
                .Where(cl => cl.Course != null)
                .Select(l => new CourseOnlyDto
                {
                    Id = l.Course!.Id,
                    CourseName = l.Course.CourseName
                }).ToList()
        });
    }
}