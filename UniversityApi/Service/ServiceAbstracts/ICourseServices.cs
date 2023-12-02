using UniversityApi.CustomResponses;
using UniversityApi.Dtos;

namespace UniversityApi.Service.ServiceAbstracts
{
    public interface ICourseServices
    {
        public Task<ApiResponse<List<CourseGetDto>>> GetCoursesAsync();
        public Task<ApiResponse<CourseGetDto>> CreateCourseAsync(CoursePostDto input);
        public Task<ApiResponse<bool>> UpdateCourseAsync(CoursePutDto input);
        public Task<ApiResponse<bool>> DeleteCourse(int courseId);
    }
}
