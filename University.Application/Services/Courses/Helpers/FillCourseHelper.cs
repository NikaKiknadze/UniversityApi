using University.Data.Data.Entities;
using University.Domain.Models.CourseModels;

namespace University.Application.Services.Courses.Helpers;

public static class FillCourseHelper
{
    public static Course FillCourse(this Course course, CoursePostDto input)
    {
        if (input.UserIds is { Count: > 0 })
            foreach (var userId in input.UserIds)
                course.UsersCourses.Add(new UsersCourses
                {
                    UserId = userId,
                    CourseId = course.Id
                });

        if (input.LecturerIds is not { Count: > 0 }) return course;
        
        foreach (var lecturerId in input.LecturerIds)
            course.CoursesLecturers.Add(new CoursesLecturersJoin
            {
                LectureId = lecturerId,
                CourseId = course.Id
            });

        return course;
    }
    
    public static Course FillCourse(this Course course, CoursePutDto input)
    {
        course.CourseName = input.CourseName;
        course.FacultyId = input.FacultyId;

        course.UsersCourses.Clear();
        
        if (input.UserIds is {Count: > 0 })
            foreach (var userId in input.UserIds)
                course.UsersCourses.Add(new UsersCourses
                {
                    UserId = userId,
                    CourseId = course.Id
                });

        course.CoursesLecturers.Clear();
        
        if (input.LecturerIds is not { Count: > 0 }) return course;
        
        foreach (var lecturerId in input.LecturerIds)
            course.CoursesLecturers.Add(new CoursesLecturersJoin
            {
                LectureId = lecturerId,
                CourseId = course.Id
            });

        return course;
    }
}