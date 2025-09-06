namespace University.Domain.Models.CourseModels;

public class CoursePostDto
{
    public string CourseName { get; set; } = string.Empty;
    public ICollection<int>? FacultyIds { get; set; }
    public ICollection<int>? LecturerIds { get; set; }
    public ICollection<int>? UserIds { get; set; }
}