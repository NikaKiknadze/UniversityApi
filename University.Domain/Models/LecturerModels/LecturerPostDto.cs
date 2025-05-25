namespace University.Domain.Models.LecturerModels;

public class LecturerPostDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public int Age { get; set; }
    public List<int>? UserIds { get; set; }
    public List<int>? CourseIds { get; set; }
}