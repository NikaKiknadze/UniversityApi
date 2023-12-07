using UniversityApi.CustomResponses;
using UniversityApi.Dtos;

namespace UniversityApi.Service.ServiceAbstracts
{
    public interface ILecturerServices
    {
        Task<ApiResponse<GetDtosWithCount<List<LecturerGetDto>>>> GetLecturersAsync(LecturerGetFilter filter, CancellationToken cancellationToken);
        Task<ApiResponse<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateLecturerAsync(LecturerPutDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteLecturerAsync(int lecturerId, CancellationToken cancellationToken);
    }
}
