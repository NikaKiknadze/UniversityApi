namespace University.Domain.Models.UserModels;

public class UserPostDto
{
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public int? FacultyId { get; set; }
    public string Password { get; set; } = null!;
    public List<int>? CourseIds { get; set; }
    public List<int>? LecturerIds { get; set; }
}