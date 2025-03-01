using University.Data.Data.Entities;
using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.AllServices.ServiceAbstracts
{
    public interface IUserServices
    {
        Task<ApiResponse<GetDtoWithCount<List<UserGetDto>>>> GetUsersAsync(UserGetFilter filter, CancellationToken cancellationToken);
        Task<ApiResponse<UserGetDto>> CreateUserAsync(UserPostDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> UpdateUserAsync(UserPutDto input, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteUserAsync(int userId, CancellationToken cancellationToken);
        List<User> FilterData(UserGetFilter filter, IQueryable<User> users);
        Task<ApiResponse<GetDtoWithCount<IEnumerable<TodosDto>>>> GetTodosInfo(TodosDto filter, CancellationToken cancellationToken);

    }
}
