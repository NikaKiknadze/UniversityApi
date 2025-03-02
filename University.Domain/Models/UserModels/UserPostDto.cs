namespace University.Domain.Models.UserModels
{
    public class UserPostDto
    {
        public string Name { get; set; } = null!;
        public string SurName { get; set; } = null!;
        public int Age { get; set; }
        public int? FacultyId { get; set; }
        public List<int>? CourseIds { get; set; }
        public List<int>? LecturerIds { get; set; }
    }
}
