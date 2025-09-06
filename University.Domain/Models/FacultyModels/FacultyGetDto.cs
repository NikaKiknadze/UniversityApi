using University.Domain.Models.CourseModels;
using University.Domain.Models.UserModels;

namespace University.Domain.Models.FacultyModels;

public class FacultyGetDto
{
    public int? Id { get; set; }
    public string? FacultyName { get; set; }
    public ICollection<UserOnlyDto> Users { get; set; } = new List<UserOnlyDto>();
    public ICollection<CourseOnlyDto> Courses { get; set; } = new List<CourseOnlyDto>();
}