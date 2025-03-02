using University.Data.Data.Entities;
using University.Domain.Models;

namespace University.Application.Services.Users.Helpers;

public static class FillDataHelper
{
    public static User FillData(this User user, UserPostDto input)
    {
        if (input.CourseIds is {Count: > 0})
            foreach (var courseId in input.CourseIds)
                user.UsersCourses.Add(new UsersCourses
                {
                    CourseId = courseId,
                    UserId = user.Id
                });

        if (input.LecturerIds is not { Count: > 0 }) return user;
        
        foreach (var lecturerId in input.LecturerIds)
            user.UsersLecturers.Add(new UsersLecturers
            {
                LecturerId = lecturerId,
                UserId = user.Id
            });

        return user;
    }
    
    public static User FillData(this User user, UserPutDto input)
    {
        user.Name = input.Name;
        user.SurName = input.Surname;
        user.Age = input.Age;
        user.FacultyId = input.FacultyId;

        user.UsersCourses.Clear();
        if (input.CourseIds is {Count: > 0})
            foreach (var courseId in input.CourseIds)
                user.UsersCourses.Add(new UsersCourses
                {
                    CourseId = courseId,
                    UserId = user.Id
                });

        user.UsersLecturers.Clear();
        if (input.LecturerIds is not { Count: > 0 }) return user;
        
        foreach (var lecturerId in input.LecturerIds)
            user.UsersLecturers.Add(new UsersLecturers
            {
                LecturerId = lecturerId,
                UserId = user.Id
            });

        return user;
    }
}