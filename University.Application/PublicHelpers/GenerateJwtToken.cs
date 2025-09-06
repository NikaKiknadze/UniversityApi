using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using University.Application.Services.Auth.Helpers;
using University.Data.Repositories.Interfaces;
using University.Domain.Models.AuthModels;

namespace University.Application.PublicHelpers;

public static class GenerateJwtToken
{
    public static async Task<AuthTokenResponse> Execute(AuthModel request, 
        IConfiguration configuration, IUserRepository userRepository,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAuthUserModel(request, cancellationToken);

        Claim[]? claims = null;
        
        if (user != null)
        {
             claims = [
                new Claim(UniversityClaimTypes.Sid, user.UserId.ToString()),
                new Claim(UniversityClaimTypes.NameIdentifier, user.Username),
                new Claim(UniversityClaimTypes.Thumbprint, user.SecurityStamp),
                new Claim(UniversityClaimTypes.Name,
                    user.Fullname?.Trim() ?? "N/A"),
                new Claim(UniversityClaimTypes.Role,
                    user.Role ?? "N/A"),
                new Claim(UniversityClaimTypes.Department,
                    user.Department ?? "N/A"),
                new Claim(UniversityClaimTypes.Extension, user.Extension ?? "N/A")
            ];
        }
        

        if (claims == null)
            throw new Exception("Token_GenError: Could not generate access token");

        var nowDate = DateTimeOffset.Now;
        var tokenExpireDate = nowDate.AddHours(150);
        var refreshTokenExpireDate =
            nowDate.AddHours(36500);

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value!));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var signInToken = new JwtSecurityTokenHandler()
            .WriteToken
            (
                new JwtSecurityToken
                (
                    configuration.GetSection("Jwt:Issuer").Value,
                    configuration.GetSection("Jwt:Audience").Value,
                    claims,
                    expires: tokenExpireDate.LocalDateTime,
                    signingCredentials: credentials
                )
            );

        AuthTokenResponse tokenResponse = new()
        {
            TokenType = "Bearer",
            AccessToken = signInToken,
            RefreshToken = string.Empty,
            ExpiresIn = tokenExpireDate.ToUnixTimeSeconds(),
            RefreshTokenExpiresIn = refreshTokenExpireDate.ToUnixTimeSeconds()
        };

        return tokenResponse;
    }
}