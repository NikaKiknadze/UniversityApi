using Microsoft.EntityFrameworkCore;
using University.Application.PublicHelpers;
using University.Data.Repositories.Interfaces;
using University.Domain.CustomExceptions;
using University.Domain.Models.AuthModels;

namespace University.Application.Services.Auth;

public class AuthServices(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IConfiguration configuration) : IAuthServices
{
    public async Task<AuthTokenResponse> Login(AuthModel request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Username))
            throw new BadRequestException("Username is required.");
        
        if (string.IsNullOrEmpty(request.Password))
            throw new BadRequestException("Password is required.");
        
        var user = await userRepository.AllAsNoTracking.SingleOrDefaultAsync(u => u.Username == request.Username, cancellationToken);
        
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            throw new AuthorizationDeniedException("Invalid credentials.");

        httpContextAccessor.HttpContext?.Session.SetInt32("UserId", user.Id);
        
        var generateToken = await GenerateJwtToken.Execute(request, configuration, userRepository, cancellationToken);
        
        return generateToken;
    }
    
    private static bool VerifyPassword(string? password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}