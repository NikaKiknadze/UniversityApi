using Microsoft.EntityFrameworkCore;
using University.Data.Data.Entities;
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
        if (!string.IsNullOrEmpty(filter.FirstName))
        {
            query.Include(u => u.UserProfile.Faculty);
            query = query.Where(u => u.UserProfile.FirstName.Contains(filter.FirstName));
        }
        if(!string.IsNullOrEmpty(filter.LastName))
        {
            query.Include(u => u.UserProfile.Faculty);
            query = query.Where(u => u.UserProfile.LastName.Contains(filter.LastName));
        }
        if(filter.Age != null)
        {
            query.Include(u => u.UserProfile.Faculty);
            query = query.Where(u => u.UserProfile.Age == filter.Age);
        }
        if(filter.FacultyId != null)
        {
            query.Include(u => u.UserProfile.Faculty);
            query = query.Where(u => u.UserProfile.FacultyId == filter.FacultyId);
        }
        if (filter.CourseIds != null && filter.CourseIds.Count != 0)
        {
            query = query.Where(u => u.UsersCourses.Any(uc => filter.CourseIds.Contains(uc.CourseId)));
        }
        if(filter.LecturerIds != null && filter.LecturerIds.Count != 0)
        {
            query = query.Where(u => u.UsersLecturers.Any(ul => filter.LecturerIds.Contains(ul.LecturerId)));
        }

        return query;
    } 
}