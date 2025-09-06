using University.Data.Data.Entities;
using University.Data.Repositories.Interfaces;
using University.Domain.Models.FacultyModels;

namespace University.Application.Services.Faculties.Helpers;

public static class FillDataHelper
{
    public static Faculty FillData(this Faculty faculty, 
        FacultyPostDto input)
    {
        faculty.FacultyCourses.Clear();
        
        if (input.CourseIds is not { Count: > 0 }) return faculty;
        
        foreach (var courseId in input.CourseIds)
            faculty.FacultyCourses.Add(new FacultyCourse
            {
                CourseId = courseId,
                FacultyId = faculty.Id
            });

        return faculty;
    }
    
    public static Faculty FillData(this Faculty faculty, 
        FacultyPutDto input)
    {
        faculty.FacultyName = input.FacultyName;
        
        faculty.FacultyCourses.Clear();
        
        if (input.CourseIds is not { Count: > 0 }) return faculty;
        
        foreach (var courseId in input.CourseIds)
            faculty.FacultyCourses.Add(new FacultyCourse
            {
                CourseId = courseId,
                FacultyId = faculty.Id
            });


        return faculty;
    }
}