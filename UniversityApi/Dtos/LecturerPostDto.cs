namespace UniversityApi.Dtos
{
    public class LecturerPostDto
    {
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public int? Age { get; set; }

        public List<int>? UserIds { get; set; }

        public List<int>? CourseIds { get; set; }
    }
}
