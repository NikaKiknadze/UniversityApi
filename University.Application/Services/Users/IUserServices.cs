using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.Services.Users
{
    public interface IUserServices
    {
        Task<ApiResponse<GetDtoWithCount<ICollection<UserGetDto>>>> Get(UserGetFilter filter, CancellationToken cancellationToken);
        Task<int> Create(UserPostDto input, CancellationToken cancellationToken);
        Task<int> Update(UserPutDto input, CancellationToken cancellationToken);
        Task<int> Delete(int userId, CancellationToken cancellationToken);
    }
}
