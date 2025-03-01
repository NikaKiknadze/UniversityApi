using University.Data.Data.Entities;
using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.AllServices.ServiceAbstracts
{
    public interface ICourseServices
    {
        Task<ApiResponse<GetDtoWithCount<List<CourseGetDto>>>> GetCoursesAsync(CourseGetFilter filter, CancellationToken cancellationToken);
        List<Course> FilterData(CourseGetFilter filter, IQueryable<Course> courses);
        Task<ApiResponse<CourseGetDto>> CreateCourseAsync(CoursePostDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateCourseAsync(CoursePutDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteCourse(int courseId, CancellationToken cancellationToken);
    }
}
