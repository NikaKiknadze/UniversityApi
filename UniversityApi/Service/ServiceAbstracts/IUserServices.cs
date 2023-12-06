using UniversityApi.CustomResponses;
using UniversityApi.Dtos;

namespace UniversityApi.Service.ServiceAbstracts
{
    public interface IUserServices
    {
        public Task<ApiResponse<UserGetDto>> GetUserByIdAsync(int userId);
        public Task<ApiResponse<List<UserGetDto>>> GetUsersAsync();
        public Task<ApiResponse<UserGetDto>> CreateUserAsync(UserPostDto input);
        public Task<ApiResponse<string>> UpdateUserAsync(UserPutDto input);
        public Task<ApiResponse<string>> DeleteUserAsync(int userId);
    }
}
