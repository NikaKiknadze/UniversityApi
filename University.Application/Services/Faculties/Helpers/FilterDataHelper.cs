using University.Data.Data.Entities;
using University.Domain.Models;
using University.Domain.Models.FacultyModels;

namespace University.Application.Services.Faculties.Helpers;

public static class FilterDataHelper
{
    public static IQueryable<Faculty> FilterData(this IQueryable<Faculty> query, FacultyGetFilter filter)
    {
        query = query.Where(f => f.IsActive == filter.IsActive);
        
        if (filter.Id != null)
        {
            query = query.Where(f => f.Id == filter.Id);
        }
        if (!string.IsNullOrEmpty(filter.FacultyName))
        {
            query = query.Where(f => f.FacultyName.Contains(filter.FacultyName));
        }
        if (filter.UserIds is {Count: > 0})
        {
            query = query.Where(f => f.Users.Any(u => filter.UserIds.Contains(u.Id)));
        }
        if (filter.CourseIds is {Count: > 0})
        {
            query = query.Where(f => f.Courses.Any(course => filter.CourseIds.Contains(course.Id)));
        }
        
        return query;
    }
}