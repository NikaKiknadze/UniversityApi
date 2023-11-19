namespace UniversityApi.Dtos
{
    public class UserPutDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public int? Age { get; set; }

        public int? FacultyId { get; set; }

        public List<int>? LecturerIds { get; set; }

        public List<int>? CourseIds { get; set; }
    }
}
