namespace University.Application.Services.Identity;

public interface IUserIdentity
{
    int UserId { get; }
    string UserName { get; }
    string FullName { get; }
}