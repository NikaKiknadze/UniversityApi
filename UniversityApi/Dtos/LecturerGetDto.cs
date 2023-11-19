namespace UniversityApi.Dtos
{
    public class LecturerGetDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public int? Age { get; set; }

        public List<int>? UserIds { get; set; }

        public List<int>? CourseIds { get; set; }
    }
}
