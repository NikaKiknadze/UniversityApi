using Microsoft.EntityFrameworkCore;
using University.Application.Services.Courses.Helpers;
using University.Data.ContextMethodsDirectory;
using University.Data.Data.Entities;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.Services.Courses
{
    public class CourseServices : ICourseServices
    {
        private readonly IUniversityContext _universityContext;
        public CourseServices(IUniversityContext universityContext)
        {
            _universityContext = universityContext;
        }

        public async Task<ApiResponse<GetDtoWithCount<ICollection<CourseGetDto>>>> Get(CourseGetFilter filter,
            CancellationToken cancellationToken)
        {
            var courses = _universityContext.Courses.AllAsNoTracking.FilterData(filter);

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
                CourseName = input.CourseName,
                FacultyId = input.FacultyId
            };

            if (string.IsNullOrEmpty(input.CourseName))
                throw new BadRequestException("Course Name is null");

            course = course.FillCourse(input);

            await _universityContext.Courses.AddAsync(course, cancellationToken);
            
            return course.Id;
        }

        public async Task<int> Update(CoursePutDto input, CancellationToken cancellationToken)
        {
            var course = await _universityContext.Courses.All
                .Include(c => c.UsersCourses)
                .ThenInclude(uc => uc.User)
                .Include(c => c.CoursesLecturers)
                .ThenInclude(cl => cl.Lecturer)
                .FirstOrDefaultAsync(c => c.Id == input.Id, cancellationToken);

            if (course is null)
                throw new BadRequestException("Course not found");

            course = course.FillCourse(input);

            await _universityContext.CompleteAsync(cancellationToken);
            
            return course.Id;
        }
        
        public async Task<int> Delete(int courseId, CancellationToken cancellationToken)
        {
            var course = await _universityContext.Courses.All
                .Include(uc => uc.UsersCourses)
                .ThenInclude(u => u.User)
                .Include(cl => cl.CoursesLecturers)
                .ThenInclude(l => l.Lecturer)
                .FirstOrDefaultAsync(c => c.Id == courseId, cancellationToken);
            
            if (course == null)
                throw new NotFoundException("Course not found");

            course.UsersCourses.Clear();
            course.CoursesLecturers.Clear();
            course.IsActive = false;
            await _universityContext.CompleteAsync(cancellationToken);
            
            return courseId;
        }
    }
}