namespace University.Domain.Models
{
    public class LecturerGetFilter : Pageing
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? SurName { get; set; }

        public int? Age { get; set; }

        public List<int>? UserIds { get; set; }

        public List<int>? CourseIds { get; set; }
    }
}
