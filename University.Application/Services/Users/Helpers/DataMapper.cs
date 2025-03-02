using University.Data.Data.Entities;
using University.Domain.Models;
namespace University.Application.Services.Users.Helpers;

public static class DataMapper
{
    public static IQueryable<UserGetDto> MapDataToUserGetDto(this IQueryable<User> users)
    {
        return users.Select(user => new UserGetDto
        {
            Id = user.Id,
            Name = user.Name,
            SurName = user.SurName,
            Age = user.Age,
            Faculty = user.Faculty != null
                ? new FacultyOnlyDto
                {
                    Id = user.Faculty.Id,
                    FacultyName = user.Faculty.FacultyName
                }
                : null,
            Courses = user.UsersCourses
                .Where(uc => uc.Course != null)
                .Select(uc => new CourseOnlyDto
                {
                    Id = uc.Course!.Id,
                    CourseName = uc.Course.CourseName
                }).ToList(),
            Lecturers = user.UsersLecturers
                .Where(ul => ul.Lecturer != null)
                .Select(ul => new LecturerOnlyDto
                {
                    Id = ul.Lecturer!.Id,
                    Name = ul.Lecturer.Name,
                    SurName = ul.Lecturer.SurName,
                    Age = ul.Lecturer.Age
                }).ToList()
        });
    }
}