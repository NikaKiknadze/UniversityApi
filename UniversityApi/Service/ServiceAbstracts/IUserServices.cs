using UniversityApi.CustomResponses;
using UniversityApi.Dtos;
using UniversityApi.Entities;

namespace UniversityApi.Service.ServiceAbstracts
{
    public interface IUserServices
    {
        Task<ApiResponse<GetDtosWithCount<List<UserGetDto>>>> GetUsersAsync(UserGetFilter filter, CancellationToken cancellationToken);
        Task<ApiResponse<UserGetDto>> CreateUserAsync(UserPostDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateUserAsync(UserPutDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteUserAsync(int userId, CancellationToken cancellationToken);
        List<User> FilterData(UserGetFilter filter, IQueryable<User> users);

    }
}
