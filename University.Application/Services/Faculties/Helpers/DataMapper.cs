using University.Data.Data.Entities;
using University.Domain.Models;
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
            Users = f.Users.Select(u => new UserOnlyDto
            {
                Id = u.Id,
                Name = u.Name,
                SurName = u.SurName,
                Age = u.Age
            }).ToList(),
            Courses = f.Courses.Select(uc => new CourseOnlyDto
            {
                Id = uc.Id,
                CourseName = uc.CourseName
            }).ToList()
        });
    }
}