namespace University.Domain.Models.UserModels
{
    public class UserGetFilter : Paging
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public int? FacultyId { get; set; }
        public bool IsActive { get; set; } = true;
        public List<int>? CourseIds { get; set; }
        public List<int>? LecturerIds { get; set; }
    }
}
