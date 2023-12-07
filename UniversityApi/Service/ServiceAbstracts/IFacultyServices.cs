using UniversityApi.CustomResponses;
using UniversityApi.Dtos;

namespace UniversityApi.Service.ServiceAbstracts
{
    public interface IFacultyServices
    {
        Task<ApiResponse<GetDtosWithCount<List<FacultyGetDto>>>> GetFacultiesAsync(FacultyGetFilter filter, CancellationToken cancellationToken);
        Task<ApiResponse<FacultyGetDto>> CreateFacultyAsync(FacultyPostDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateFacultyAsync(FacultyPutDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteFacultyAsync(int facultyId, CancellationToken cancellationToken);
    }
}
