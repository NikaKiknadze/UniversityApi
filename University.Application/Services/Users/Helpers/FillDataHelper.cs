using University.Data.Data.Entities;
using University.Domain.Models.UserModels;

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
        user.UserProfile ??= new UserProfile
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Age = input.Age,
            FacultyId = input.FacultyId
        };
        
        user.UserProfile.FirstName = input.FirstName;
        user.UserProfile.LastName = input.LastName;
        user.UserProfile.Age = input.Age;
        user.UserProfile.FacultyId = input.FacultyId;

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