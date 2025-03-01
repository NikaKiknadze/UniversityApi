namespace University.Domain.Models
{
    public class CoursePostDto
    {
        public string? CourseName { get; set; }

        public int? FacultyId { get; set; }

        public List<int>? LecturerIds { get; set; }

        public List<int>? UserIds { get; set; }
    }
}
