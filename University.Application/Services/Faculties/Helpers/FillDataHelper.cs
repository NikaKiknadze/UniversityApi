using Microsoft.EntityFrameworkCore;
using University.Data.ContextMethodsDirectory;
using University.Data.Data.Entities;
using University.Domain.Models;
using University.Domain.Models.FacultyModels;

namespace University.Application.Services.Faculties.Helpers;

public static class FillDataHelper
{
    public static async Task<Faculty> FillData(this Faculty faculty, 
        FacultyPostDto input, IUniversityContext universityContext,
        CancellationToken cancellationToken)
    {
        if (input.UserIds is { Count: > 0 })
        {
            var users = await universityContext.Users.All.Where(user => input.UserIds.Contains(user.Id))
                .ToListAsync(cancellationToken);

            if (users.Count != 0)
                foreach (var user in users)
                {
                    user.FacultyId = faculty.Id;
                    user.FacultyId = faculty.Id;
                    faculty.Users.Add(user);
                }
        }

        if (input.CourseIds is not { Count: > 0 }) return faculty;
        var courses = await universityContext.Courses.All.Where(course => input.CourseIds.Contains(course.Id))
            .ToListAsync(cancellationToken);

        if (courses.Count == 0) return faculty;
        {
            foreach (var course in courses)
            {
                course.FacultyId = faculty.Id;
                faculty.Courses.Add(course);
            }
        }

        return faculty;
    }
    
    public static async Task<Faculty> FillData(this Faculty faculty, 
        FacultyPutDto input, IUniversityContext universityContext,
        CancellationToken cancellationToken)
    {
        faculty.FacultyName = input.FacultyName;

        faculty.Users.Clear();
        if (input.UserIds is {Count: > 0})
        {
            var users = await universityContext.Users.All.Where(user => input.UserIds.Contains(user.Id))
                .ToListAsync(cancellationToken);

            if (users.Count != 0)
                foreach (var user in users)
                {
                    user.FacultyId = faculty.Id;
                    user.FacultyId = faculty.Id;
                    faculty.Users.Add(user);
                }
        }

        faculty.Courses.Clear();
        
        if (input.CourseIds is not { Count: > 0 }) return faculty;
        
        var courses = await universityContext.Courses.All.Where(course => input.CourseIds.Contains(course.Id))
            .ToListAsync(cancellationToken);

        if (courses.Count == 0) return faculty;
            foreach (var course in courses)
            {
                course.FacultyId = faculty.Id;
                faculty.Courses.Add(course);
            }


        return faculty;
    }
}