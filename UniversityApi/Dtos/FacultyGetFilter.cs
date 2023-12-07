namespace UniversityApi.Dtos
{
    public class FacultyGetFilter: Pageing
    {
        public int? Id { get; set; }

        public string? FacultyName { get; set; }

        public List<int>? UserIds { get; set; }

        public List<int>? CourseIds { get; set; }
    }
}
