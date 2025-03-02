using University.Data.Data.Entities;
using University.Domain.Models;
using University.Domain.Models.UserModels;

namespace University.Application.Services.Users.Helpers;

public static class FilterDataHelper
{
    public static IQueryable<User> FilterData(this IQueryable<User> query, UserGetFilter filter)
    {
        query = query.Where(u => u.IsActive == filter.IsActive);
        
        if (filter.Id != null)
        {
            query = query.Where(u => u.Id == filter.Id);
        }
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(u => u.Name.Contains(filter.Name));
        }
        if(!string.IsNullOrEmpty(filter.SurName))
        {
            query = query.Where(u => u.SurName.Contains(filter.SurName));
        }
        if(filter.Age != null)
        {
            query = query.Where(u => u.Age == filter.Age);
        }
        if(filter.FacultyId != null)
        {
            query = query.Where(u => u.FacultyId == filter.FacultyId);
        }
        if (filter.CourseIds != null && filter.CourseIds.Any())
        {
            query = query.Where(u => u.UsersCourses.Any(uc => filter.CourseIds.Contains(uc.CourseId)));
        }
        if(filter.LecturerIds != null && filter.LecturerIds.Any())
        {
            query = query.Where(u => u.UsersLecturers.Any(ul => filter.LecturerIds.Contains(ul.LecturerId)));
        }

        return query;
    } 
}