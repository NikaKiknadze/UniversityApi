using University.Domain.Models.CourseModels;
using University.Domain.Models.UserModels;

namespace University.Domain.Models.FacultyModels;

public class FacultyGetDto
{
    public int? Id { get; set; }
    public string? FacultyName { get; set; }
    public List<UserOnlyDto>? Users { get; set; }
    public List<CourseOnlyDto>? Courses { get; set; }
}