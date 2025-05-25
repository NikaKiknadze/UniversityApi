using Microsoft.EntityFrameworkCore;
using University.Data.ContextMethodsDirectory;
using University.Domain.Models.AuthModels;

namespace University.Application.Services.Auth.Helpers;

public static class UserResultForAuthToken
{
    public static async Task<SignInUserModel?> GetAuthUserModel(this IUniversityContext universityContext,
        AuthModel request,
        CancellationToken cancellationToken)
    {
        return await universityContext.Users.All
            .Include(u => u.UserProfile)
            .Where(u => u.Username == request.Username && 
                        u.UserProfile != null)
            .AsNoTracking()
            .Select(x => new SignInUserModel
            {
                UserId = x.Id,
                Username = x.Username,
                Fullname = x.UserProfile!.FirstName + " " + x.UserProfile.LastName,
                SecurityStamp = x.PasswordHash,
                IsActive = x.IsActive,
                Password = x.PasswordHash,
                ReadyTime = null,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}