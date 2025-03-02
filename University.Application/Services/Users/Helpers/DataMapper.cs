using University.Data.Data.Entities;
using University.Domain.Models.CourseModels;
using University.Domain.Models.FacultyModels;
using University.Domain.Models.LecturerModels;
using University.Domain.Models.UserModels;

namespace University.Application.Services.Users.Helpers;

public static class DataMapper
{
    public static IQueryable<UserGetDto> MapDataToUserGetDto(this IQueryable<User> users)
    {
        return users.Select(user => new UserGetDto
        {
            Id = user.Id,
            FirstName = user.UserProfile.FirstName,
            LastName = user.UserProfile.LastName,
            Age = user.UserProfile.Age,
            Faculty = user.UserProfile.Faculty != null
                ? new FacultyOnlyDto
                {
                    Id = user.UserProfile.Faculty.Id,
                    FacultyName = user.UserProfile.Faculty.FacultyName
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