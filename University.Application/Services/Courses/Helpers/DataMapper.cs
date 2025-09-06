using University.Data.Data.Entities;
using University.Domain.Models.CourseModels;
using University.Domain.Models.FacultyModels;
using University.Domain.Models.LecturerModels;
using University.Domain.Models.UserModels;

namespace University.Application.Services.Courses.Helpers;

public static class DataMapper
{
    public static IQueryable<CourseGetDto> MapToCourseGetDto(this IQueryable<Course> courses)
    {
        return courses.Select(course => new CourseGetDto
        {
            Id = course.Id,
            Faculties = course.FacultyCourses
                .Where(ul => ul.Faculty.IsActive)
                .Select(c => new FacultyOnlyDto
                {
                    Id = c.FacultyId,
                    FacultyName = c.Faculty.FacultyName
                }).ToList(),
            Lecturers = course.CoursesLecturers
                .Where(x => x.Course.IsActive)
                .Select(c => new LecturerOnlyDto
                {
                    Id = c.Lecturer.Id,
                    Name = c.Lecturer.Name,
                    SurName = c.Lecturer.SurName,
                    Age = c.Lecturer.Age
                }).ToList(),
            Users = course.UsersCourses
                .Where(uc => uc.User.IsActive)
                .Select(c => new UserOnlyDto
                {
                    Id = c.User.Id,
                    FirstName = c.User.UserProfile.FirstName,
                    LastName = c.User.UserProfile.LastName,
                    Age = c.User.UserProfile.Age
                }).ToList()
        });
    }
}