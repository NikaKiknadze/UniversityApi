using University.Domain.CustomResponses;
using University.Domain.Models;
using University.Domain.Models.CourseModels;

namespace University.Application.Services.Courses;

public interface ICourseServices
{
    Task<ApiResponse<GetDtoWithCount<ICollection<CourseGetDto>>>> Get(CourseGetFilter filter, CancellationToken cancellationToken);
    Task<int> Create(CoursePostDto input, CancellationToken cancellationToken);
    Task<int> Update(CoursePutDto input, CancellationToken cancellationToken);
    Task<int> Delete(int courseId, CancellationToken cancellationToken);
}