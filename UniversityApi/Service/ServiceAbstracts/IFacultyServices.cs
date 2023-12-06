using UniversityApi.CustomResponses;
using UniversityApi.Dtos;

namespace UniversityApi.Service.ServiceAbstracts
{
    public interface IFacultyServices
    {
        public Task<ApiResponse<FacultyGetDto>> GetFacultyByIdAsync(int facultyId);
        public Task<ApiResponse<List<FacultyGetDto>>> GetFacultiesAsync();
        public Task<ApiResponse<FacultyGetDto>> CreateFacultyAsync(FacultyPostDto input);
        public Task<ApiResponse<string>> UpdateFacultyAsync(FacultyPutDto input);
        public Task<ApiResponse<string>> DeleteFacultyAsync(int facultyId);
    }
}
