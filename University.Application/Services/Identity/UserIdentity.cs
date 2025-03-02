using University.Domain.Models.AuthModels;

namespace University.Application.Services.Identity;

public class UserIdentity : IUserIdentity
{
    public UserIdentity(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null || !context!.User.Identity!.IsAuthenticated) return;
        UserId = int.TryParse(context.User.Claims.FirstOrDefault(c => c.Type == UniversityClaimTypes.Sid)?.Value, out var userId) ? userId : 0;
        UserName = context.User.Claims.FirstOrDefault(c => c.Type == UniversityClaimTypes.NameIdentifier)?.Value;
        SecurityStamp = context.User.Claims.FirstOrDefault(c => c.Type == UniversityClaimTypes.Thumbprint)?.Value;
        FullName = context.User.Claims.FirstOrDefault(c => c.Type == UniversityClaimTypes.Name)?.Value;
    }
    
    public int UserId { get; }
    public string? UserName { get; }
    public string? FullName { get; }
    public string? SecurityStamp { get; private set; }
    
}