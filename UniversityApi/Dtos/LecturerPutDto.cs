namespace UniversityApi.Dtos
{
    public class LecturerPutDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public int? Age { get; set; }

        public List<int>? UserIds { get; set; }

        public List<int>? CourseIds { get; set; }
    }
}
