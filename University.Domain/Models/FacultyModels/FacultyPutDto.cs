namespace University.Domain.Models.FacultyModels;

public class FacultyPutDto
{
    public int Id { get; set; }
    public string FacultyName { get; set; } = null!;
    public ICollection<int>? CourseIds { get; set; }
}