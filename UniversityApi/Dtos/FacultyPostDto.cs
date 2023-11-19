namespace UniversityApi.Dtos
{
    public class FacultyPostDto
    {
        public string? FacultyName { get; set; }

        public List<int>? UserIds { get; set; }

        public List<int>? CourseIds { get; set; }
    }
}
