namespace University.Domain.Models.CourseModels;

public class CoursePostDto
{
    public string CourseName { get; set; } = string.Empty;
    public int? FacultyId { get; set; }
    public List<int>? LecturerIds { get; set; }
    public List<int>? UserIds { get; set; }
}