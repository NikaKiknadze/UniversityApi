using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.AllServices.ServiceAbstracts
{
    public interface ILecturerServices
    {
        Task<ApiResponse<GetDtoWithCount<List<LecturerGetDto>>>> GetLecturersAsync(LecturerGetFilter filter, CancellationToken cancellationToken);
        Task<ApiResponse<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateLecturerAsync(LecturerPutDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteLecturerAsync(int lecturerId, CancellationToken cancellationToken);
    }
}
