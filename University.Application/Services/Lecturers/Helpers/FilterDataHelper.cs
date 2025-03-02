using University.Data.Data.Entities;
using University.Domain.Models;
using University.Domain.Models.LecturerModels;

namespace University.Application.Services.Lecturers.Helpers;

public static class FilterDataHelper
{
    public static IQueryable<Lecturer> FilterData(this IQueryable<Lecturer> query, LecturerGetFilter filter)
    {
        query = query.Where(l => l.IsActive == filter.IsActive);
        if(filter.Id != null)
        {
            query = query.Where(l => l.Id == filter.Id);
        }
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(l => l.Name.Contains(filter.Name));
        }
        if (!string.IsNullOrEmpty(filter.SurName))
        {
            query = query.Where(l => l.SurName.Contains(filter.SurName));
        }
        if(filter.Age != null)
        {
            query = query.Where(l => l.Age == filter.Age);
        }
        if(filter.UserIds != null && filter.UserIds.Any())
        {
            query = query.Where(l => l.UsersLecturers.Any(ul => filter.UserIds.Contains(ul.LecturerId)));
        }
        if(filter.CourseIds != null && filter.CourseIds.Any())
        {
            query = query.Where(l => l.CoursesLecturers.Any(cl => filter.CourseIds.Contains(cl.CourseId)));
        }

        return query;
    }   
}