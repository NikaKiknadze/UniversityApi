namespace University.Domain.Models
{
    public class CourseGetDto
    {
        public int? Id { get; init; }

        public string? CourseName { get; set; }

        public FacultyOnlyDto? Faculty { get; set; }

        public List<LecturerOnlyDto>? Lecturers { get; set; }

        public List<UserOnlyDto>? Users { get; set; }
    }
}
