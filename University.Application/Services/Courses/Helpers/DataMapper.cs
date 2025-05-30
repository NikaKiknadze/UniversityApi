﻿using University.Data.Data.Entities;
using University.Domain.Models.CourseModels;
using University.Domain.Models.FacultyModels;
using University.Domain.Models.LecturerModels;
using University.Domain.Models.UserModels;

namespace University.Application.Services.Courses.Helpers;

public static class DataMapper
{
    public static IQueryable<CourseGetDto> MapToCourseGetDto(this IQueryable<Course> courses)
    {
        return courses.Select(course => new CourseGetDto
        {
            Id = course.Id,
            Faculty = course.Faculty != null
                ? new FacultyOnlyDto
                {
                    Id = course.Faculty.Id,
                    FacultyName = course.Faculty.FacultyName
                }
                : null,
            Lecturers = course.CoursesLecturers
                .Where(ul => ul.Lecturer != null)
                .Select(c => new LecturerOnlyDto
                {
                    Id = c.Lecturer!.Id,
                    Name = c.Lecturer.Name,
                    SurName = c.Lecturer.SurName,
                    Age = c.Lecturer.Age
                }).ToList(),
            Users = course.UsersCourses
                .Where(uc => uc.User != null && 
                             uc.User.IsActive == true)
                .Select(c => new UserOnlyDto
                {
                    Id = c.User!.Id,
                    FirstName = c.User.UserProfile.FirstName,
                    LastName = c.User.UserProfile.LastName,
                    Age = c.User.UserProfile.Age
                }).ToList()
        });
    }
}