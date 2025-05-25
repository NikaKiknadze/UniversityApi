namespace University.Domain.Models.CourseModels;

public class CoursePutDto
{
    public int Id { get; set; }
    public string CourseName { get; set; } = null!;
    public int? FacultyId { get; set; }
    public List<int>? LecturerIds { get; set; }
    public List<int>? UserIds { get; set; }
}