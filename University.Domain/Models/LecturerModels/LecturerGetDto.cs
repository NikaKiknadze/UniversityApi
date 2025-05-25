using University.Domain.Models.CourseModels;
using University.Domain.Models.UserModels;

namespace University.Domain.Models.LecturerModels;

public class LecturerGetDto
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string SurName { get; set; }
    public required int Age { get; set; }
    public List<UserOnlyDto>? Users { get; set; }
    public List<CourseOnlyDto>? Courses { get; set; }
}