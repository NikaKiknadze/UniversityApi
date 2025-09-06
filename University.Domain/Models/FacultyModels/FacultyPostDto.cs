namespace University.Domain.Models.FacultyModels;

public class FacultyPostDto
{
    public string FacultyName { get; set; } = null!;
    public ICollection<int>? CourseIds { get; set; }
}