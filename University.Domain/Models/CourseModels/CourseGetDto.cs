using University.Domain.Models.FacultyModels;
using University.Domain.Models.LecturerModels;
using University.Domain.Models.UserModels;

namespace University.Domain.Models.CourseModels;

public class CourseGetDto
{
    public int Id { get; set; }
    public ICollection<FacultyOnlyDto> Faculties { get; set; } = new List<FacultyOnlyDto>();
    public ICollection<LecturerOnlyDto> Lecturers { get; set; } = new List<LecturerOnlyDto>();
    public ICollection<UserOnlyDto> Users { get; set; } = new List<UserOnlyDto>();
}