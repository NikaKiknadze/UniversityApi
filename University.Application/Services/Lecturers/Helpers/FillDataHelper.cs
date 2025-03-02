using University.Data.Data.Entities;
using University.Domain.Models;
using University.Domain.Models.LecturerModels;

namespace University.Application.Services.Lecturers.Helpers;

public static class FillDataHelper
{
    public static Lecturer FillData(this Lecturer lecturer, LecturerPostDto input)
    {
        if (input.CourseIds is {Count: > 0})
            foreach (var courseId in input.CourseIds)
                lecturer.CoursesLecturers.Add(new CoursesLecturersJoin()
                {
                    CourseId = courseId,
                    LectureId = lecturer.Id
                });

        if (input.UserIds is not { Count: > 0 }) return lecturer;
        
        foreach (var userId in input.UserIds)
            lecturer.UsersLecturers.Add(new UsersLecturers()
            {
                UserId = userId,
                LecturerId = lecturer.Id
            });

        return lecturer;
    }
    
    public static Lecturer FillData(this Lecturer lecturer, LecturerPutDto input)
    {
        lecturer.Name = input.Name;
        lecturer.SurName = input.Surname;
        lecturer.Age = input.Age;
        
        lecturer.CoursesLecturers.Clear();
        if (input.CourseIds is {Count: > 0})
            foreach (var courseId in input.CourseIds)
                lecturer.CoursesLecturers.Add(new CoursesLecturersJoin()
                {
                    CourseId = courseId,
                    LectureId = lecturer.Id
                });

        lecturer.UsersLecturers.Clear();
        if (input.UserIds is not { Count: > 0 }) return lecturer;
        
        foreach (var userId in input.UserIds)
            lecturer.UsersLecturers.Add(new UsersLecturers()
            {
                UserId = userId,
                LecturerId = lecturer.Id
            });

        return lecturer;
    }
}