namespace University.Domain.Models.LecturerModels;

public class LecturerPutDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public int Age { get; set; }
    public ICollection<int>? UserIds { get; set; }
    public ICollection<int>? CourseIds { get; set; }
}