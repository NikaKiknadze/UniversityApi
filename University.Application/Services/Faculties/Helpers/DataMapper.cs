using University.Data.Data.Entities;
using University.Domain.Models.CourseModels;
using University.Domain.Models.FacultyModels;
using University.Domain.Models.UserModels;

namespace University.Application.Services.Faculties.Helpers;

public static class DataMapper
{
    public static IQueryable<FacultyGetDto> MapToFacultyGetDto(this IQueryable<Faculty> faculties)
    {
        return faculties.Select(f => new FacultyGetDto
        {
            Id = f.Id,
            FacultyName = f.FacultyName,
            Users = f.Users
                .Where(x => x.User.IsActive)
                .Select(u => new UserOnlyDto
            {
                Id = u.UserId,
                FirstName = u.User.UserProfile.FirstName,
                LastName = u.User.UserProfile.LastName,
                Age = u.User.UserProfile.Age
            }).ToList(),
            Courses = f.FacultyCourses
                .Where(x => x.Course.IsActive)
                .Select(uc => new CourseOnlyDto
            {
                Id = uc.CourseId,
                CourseName = uc.Course.CourseName
            }).ToList()
        });
    }
}