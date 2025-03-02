using University.Domain.Models.FacultyModels;
using University.Domain.Models.LecturerModels;
using University.Domain.Models.UserModels;

namespace University.Domain.Models.CourseModels
{
    public class CourseGetDto
    {
        public int? Id { get; init; }
        public FacultyOnlyDto? Faculty { get; set; }
        public List<LecturerOnlyDto>? Lecturers { get; set; }
        public List<UserOnlyDto>? Users { get; set; }
    }
}
