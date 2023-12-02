using UniversityApi.CustomResponses;
using UniversityApi.Dtos;

namespace UniversityApi.Service.ServiceAbstracts
{
    public interface ILecturerServices
    {
        public Task<ApiResponse<List<LecturerGetDto>>> GetLecturersAsync();
        public Task<ApiResponse<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input);
        public Task<ApiResponse<bool>> UpdateLecturerAsync(LecturerPutDto input);
        public Task<ApiResponse<bool>> DeleteLecturerAsync(int lecturerId);
    }
}
