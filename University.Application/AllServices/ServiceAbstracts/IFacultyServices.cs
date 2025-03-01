using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.AllServices.ServiceAbstracts
{
    public interface IFacultyServices
    {
        Task<ApiResponse<GetDtoWithCount<List<FacultyGetDto>>>> GetFacultiesAsync(FacultyGetFilter filter, CancellationToken cancellationToken);
        Task<ApiResponse<FacultyGetDto>> CreateFacultyAsync(FacultyPostDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateFacultyAsync(FacultyPutDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteFacultyAsync(int facultyId, CancellationToken cancellationToken);
    }
}
