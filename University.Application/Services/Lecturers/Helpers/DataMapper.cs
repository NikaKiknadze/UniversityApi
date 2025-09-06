using University.Data.Data.Entities;
using University.Domain.Models.CourseModels;
using University.Domain.Models.LecturerModels;
using University.Domain.Models.UserModels;

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
                .Where(ul => ul.User.IsActive)
                .Select(l => new UserOnlyDto
                {
                    Id = l.User.Id,
                    FirstName = l.User.UserProfile.FirstName,
                    LastName = l.User.UserProfile.LastName,
                    Age = l.User.UserProfile.Age
                }).ToList(),
            Courses = lecturer.CoursesLecturers
                .Where(cl => cl.Course.IsActive)
                .Select(l => new CourseOnlyDto
                {
                    Id = l.Course.Id,
                    CourseName = l.Course.CourseName
                }).ToList()
        });
    }
}