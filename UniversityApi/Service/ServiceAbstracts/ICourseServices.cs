using UniversityApi.CustomResponses;
using UniversityApi.Dtos;
using UniversityApi.Entities;

namespace UniversityApi.Service.ServiceAbstracts
{
    public interface ICourseServices
    {
        Task<ApiResponse<GetDtosWithCount<List<CourseGetDto>>>> GetCoursesAsync(CourseGetFilter filter, CancellationToken cancellationToken);
        List<Course> FilterData(CourseGetFilter filter, IQueryable<Course> courses);
        Task<ApiResponse<CourseGetDto>> CreateCourseAsync(CoursePostDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateCourseAsync(CoursePutDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteCourse(int courseId, CancellationToken cancellationToken);
    }
}
