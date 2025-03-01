namespace University.Domain.Models
{
    public class LecturerGetDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? SurName { get; set; }

        public int? Age { get; set; }

        public List<UserOnlyDto>? Users { get; set; }

        public List<CourseOnlyDto>? Courses { get; set; }
    }
}
