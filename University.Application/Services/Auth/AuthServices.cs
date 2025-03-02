using Microsoft.EntityFrameworkCore;
using University.Application.PublicHelpers;
using University.Data.ContextMethodsDirectory;
using University.Domain.Models.AuthModels;

namespace University.Application.Services.Auth;

public class AuthServices(IUniversityContext universityContext, IConfiguration configuration) : IAuthServices
{
    public async Task<AuthTokenResponse> Login(AuthModel request, CancellationToken cancellationToken)
    {
        var user = await universityContext.Users.AllAsNoTracking.SingleOrDefaultAsync(u => u.Username == request.Username, cancellationToken);
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var generateToken = await GenerateJwtToken.Execute(request, configuration, universityContext, cancellationToken);
        
        return generateToken;
    }
    
    private static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}