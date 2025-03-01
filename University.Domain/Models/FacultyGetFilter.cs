namespace University.Domain.Models
{
    public class FacultyGetFilter: Pageing
    {
        public int? Id { get; set; }

        public string? FacultyName { get; set; }

        public List<int>? UserIds { get; set; }

        public List<int>? CourseIds { get; set; }
    }
}
