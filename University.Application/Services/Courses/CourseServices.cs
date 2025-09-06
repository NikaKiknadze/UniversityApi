using Microsoft.EntityFrameworkCore;
using University.Application.Services.Courses.Helpers;
using University.Data.Data.Entities;
using University.Data.Repositories.Interfaces;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;
using University.Domain.Models;
using University.Domain.Models.CourseModels;

namespace University.Application.Services.Courses;

public class CourseServices(ICourseRepository courseRepository) : ICourseServices
{
    public async Task<ApiResponse<GetDtoWithCount<ICollection<CourseGetDto>>>> Get(CourseGetFilter filter,
        CancellationToken cancellationToken)
    {
        var courses = courseRepository.AllAsNoTracking.FilterData(filter);

        var result = await courses
            .MapToCourseGetDto()
            .AsSplitQuery()
            .OrderByDescending(c => c.Id)
            .Skip(filter.Offset ?? 0)
            .Take(filter.Limit ?? 10)
            .ToListAsync(cancellationToken);

        return ApiResponse<GetDtoWithCount<ICollection<CourseGetDto>>>.SuccessResult(new GetDtoWithCount<ICollection<CourseGetDto>>
        {
            Data = result,
            Count = await courses.CountAsync(cancellationToken)
        });
    }

    public async Task<int> Create(CoursePostDto input, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            CourseName = input.CourseName
        };

        if (string.IsNullOrEmpty(input.CourseName))
            throw new BadRequestException("Course Name is null");

        course = course.FillCourse(input);

        await courseRepository.AddAsync(course, cancellationToken);
            
        return course.Id;
    }

    public async Task<int> Update(CoursePutDto input, CancellationToken cancellationToken)
    {
        var course = await courseRepository.All
            .Include(c => c.UsersCourses)
            .Include(c => c.CoursesLecturers)
            .Include(x => x.FacultyCourses)
            .FirstOrDefaultAsync(c => c.Id == input.Id, cancellationToken);

        if (course is null)
            throw new BadRequestException("Course not found");

        course = course.FillCourse(input);

        await courseRepository.UpdateAsync(course ,cancellationToken);
            
        return course.Id;
    }
        
    public async Task<int> Delete(int courseId, CancellationToken cancellationToken)
    {
        var course = await courseRepository.All
            .Include(uc => uc.UsersCourses)
            .Include(cl => cl.CoursesLecturers)
            .Include(x => x.FacultyCourses)
            .FirstOrDefaultAsync(c => c.Id == courseId, cancellationToken);
            
        if (course == null)
            throw new NotFoundException("Course not found");

        course.UsersCourses.Clear();
        course.CoursesLecturers.Clear();
        course.FacultyCourses.Clear();
        course.IsActive = false;
        
        await courseRepository.UpdateAsync(course ,cancellationToken);
            
        return courseId;
    }
}