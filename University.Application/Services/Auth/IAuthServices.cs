using University.Domain.Models.AuthModels;

namespace University.Application.Services.Auth;

public interface IAuthServices
{
    Task<AuthTokenResponse> Login(AuthModel request, CancellationToken cancellationToken);
}