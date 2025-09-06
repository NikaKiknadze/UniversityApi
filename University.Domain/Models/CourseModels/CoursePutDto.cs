namespace University.Domain.Models.CourseModels;

public class CoursePutDto
{
    public int Id { get; set; }
    public string CourseName { get; set; } = null!;
    public ICollection<int>? FacultyIds { get; set; }
    public ICollection<int>? LecturerIds { get; set; }
    public ICollection<int>? UserIds { get; set; }
}