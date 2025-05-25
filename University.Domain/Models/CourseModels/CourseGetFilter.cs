namespace University.Domain.Models.CourseModels;

public class CourseGetFilter : Paging
{
    public int? Id { get; set; }
    public bool IsActive { get; set; } = true;
    public string? CourseName { get; set; }
    public int? FacultyId { get; set; }
    public List<int>? LecturerIds { get; set; }
    public List<int>? UserIds { get; set; }
}