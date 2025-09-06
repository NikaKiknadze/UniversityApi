using University.Domain.Models.CourseModels;
using University.Domain.Models.UserModels;

namespace University.Domain.Models.LecturerModels;

public class LecturerGetDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SurName { get; set; }
    public required int Age { get; set; }
    public ICollection<UserOnlyDto> Users { get; set; } = new List<UserOnlyDto>();
    public ICollection<CourseOnlyDto> Courses { get; set; } = new List<CourseOnlyDto>();
}