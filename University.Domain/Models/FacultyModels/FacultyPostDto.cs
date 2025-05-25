namespace University.Domain.Models.FacultyModels;

public class FacultyPostDto
{
    public string FacultyName { get; set; } = null!;
    public List<int>? CourseIds { get; set; }
}