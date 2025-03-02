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
            // .Include(u => u.UserDepartments)
            // .Include(u => u.UserRoles)
            .Where(u => u.Username == request.Username && 
                        u.UserProfile != null)
            .AsNoTracking()
            .Select(x => new SignInUserModel()
            {
                UserId = x.Id,
                Username = x.Username,
                Fullname = x.UserProfile!.FirstName + " " + x.UserProfile.LastName,
                SecurityStamp = x.PasswordHash,
                // Role = x.UserRoles.Select(r => r.Role.Name).FirstOrDefault(),
                IsActive = x.IsActive,
                Password = x.PasswordHash,
                // Department = x.UserDepartments.Select(d => d.DepartmentId).FirstOrDefault().ToString(),
                // Extension = x.UserProfile.SipNumber,
                // AvatarImage = x.UserProfile.AvatarImage,
                ReadyTime = null,
                // Language = x.UserProfile.Language.Code,
                // JobPosition = x.UserProfile.Position != null && x.UserProfile.Position.PositionName != null
                //     ? x.UserProfile
                //         .Position.PositionName.ToString()
                //     : null
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}