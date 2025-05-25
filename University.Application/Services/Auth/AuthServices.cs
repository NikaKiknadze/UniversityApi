using Microsoft.EntityFrameworkCore;
using University.Application.PublicHelpers;
using University.Data.ContextMethodsDirectory;
using University.Domain.CustomExceptions;
using University.Domain.Models.AuthModels;

namespace University.Application.Services.Auth;

public class AuthServices(IHttpContextAccessor httpContextAccessor, IUniversityContext universityContext, IConfiguration configuration) : IAuthServices
{
    public async Task<AuthTokenResponse> Login(AuthModel request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Username))
            throw new BadRequestException("Username is required.");
        
        if (string.IsNullOrEmpty(request.Password))
            throw new BadRequestException("Password is required.");
        
        var user = await universityContext.Users.AllAsNoTracking.SingleOrDefaultAsync(u => u.Username == request.Username, cancellationToken);
        
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            throw new AuthorizationDeniedException("Invalid credentials.");

        httpContextAccessor.HttpContext?.Session.SetInt32("UserId", user.Id);
        
        var generateToken = await GenerateJwtToken.Execute(request, configuration, universityContext, cancellationToken);
        
        return generateToken;
    }
    
    private static bool VerifyPassword(string? password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}