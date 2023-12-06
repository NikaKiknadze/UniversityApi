using UniversityApi.CustomResponses;
using UniversityApi.Dtos;

namespace UniversityApi.Service.ServiceAbstracts
{
    public interface ILecturerServices
    {
        public Task<ApiResponse<List<LecturerGetDto>>> GetLecturersAsync();
        public Task<ApiResponse<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input);
        public Task<ApiResponse<string>> UpdateLecturerAsync(LecturerPutDto input);
        public Task<ApiResponse<string>> DeleteLecturerAsync(int lecturerId);
    }
}
