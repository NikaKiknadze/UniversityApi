namespace University.Domain.Models.UserModels
{
    public class UserPutDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public int Age { get; set; }
        public int? FacultyId { get; set; }
        public List<int>? LecturerIds { get; set; }
        public List<int>? CourseIds { get; set; }
    }
}
