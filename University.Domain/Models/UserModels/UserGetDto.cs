using University.Domain.Models.CourseModels;
using University.Domain.Models.FacultyModels;
using University.Domain.Models.LecturerModels;

namespace University.Domain.Models.UserModels;

public class UserGetDto
{
    public int Id { get; init; }

    public required string FirstName { get; set; }
        
    public required string LastName { get; set; }
        
    public required int Age { get; set; }
        
    public FacultyOnlyDto? Faculty { get; set; }
        
    public List<LecturerOnlyDto>? Lecturers { get; set; }
        
    public List<CourseOnlyDto>? Courses { get; set; }
}