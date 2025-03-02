using Microsoft.EntityFrameworkCore;
using University.Data.Data.Entities;
using University.Domain.Models;

namespace University.Application.Services.Courses.Helpers;

public static class FilterDataHelper
{
    public static IQueryable<Course> FilterData(this IQueryable<Course> query, CourseGetFilter filter)
    {
        query = query.Where(c => c.IsActive == CourseGetFilter.IsActive);
        
        if(filter.Id != null)
        {
            query = query.Where(c => c.Id == filter.Id);
        }
        if (!string.IsNullOrEmpty(filter.CourseName))
        {
            query = query.Where(c => c.CourseName.Contains(filter.CourseName));
        }
        if(filter.FacultyId != null)
        {
            query = query.Where(c => c.FacultyId == filter.FacultyId);
        }
        if(filter.LecturerIds != null && filter.LecturerIds.Any())
        {
            query.Include(c => c.CoursesLecturers);
            query = query.Where(c => c.CoursesLecturers.Any(cl => filter.LecturerIds.Contains(cl.LectureId)));
        }
        if (filter.UserIds == null || !filter.UserIds.Any()) return query;
        {
            query.Include(c => c.UsersCourses);
            query = query.Where(c => c.UsersCourses.Any(uc => filter.UserIds.Contains(uc.CourseId)));
        }

        return query;
    }
}