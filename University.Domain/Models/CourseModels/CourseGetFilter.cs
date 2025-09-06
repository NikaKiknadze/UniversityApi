namespace University.Domain.Models.CourseModels;

public class CourseGetFilter : Paging
{
    public int? Id { get; set; }
    public bool IsActive { get; set; } = true;
    public string? CourseName { get; set; }
    public ICollection<int>? FacultyIds { get; set; }
    public ICollection<int>? LecturerIds { get; set; }
    public ICollection<int>? UserIds { get; set; }
}