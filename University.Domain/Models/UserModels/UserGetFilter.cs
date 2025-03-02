namespace University.Domain.Models.UserModels
{
    public class UserGetFilter : Paging
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public int? Age { get; set; }
        public int? FacultyId { get; set; }
        public bool IsActive { get; set; } = true;
        public List<int>? CourseIds { get; set; }
        public List<int>? LecturerIds { get; set; }
    }
}
