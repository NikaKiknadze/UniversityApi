namespace University.Domain.Models.AuthModels;

public class SignInUserModel
{
    public int UserId { get; set; }
    public string? Role { get; set; }
    public bool? IsActive { get; set; }
    public string Password { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string SecurityStamp { get; set; } = null!;
    public string? Department { get; set; }
    public string? Fullname { get; set; }
    public string? Extension { get; set; }
    public string? AvatarImage { get; set; }
    public long? ReadyTime { get; set; }
    public string? Language { get; set; }
    public string? JobPosition { get; set; }
}