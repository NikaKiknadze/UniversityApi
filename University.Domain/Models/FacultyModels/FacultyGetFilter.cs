namespace University.Domain.Models.FacultyModels
{
    public class FacultyGetFilter : Paging
    {
        public int? Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string? FacultyName { get; set; }
        public List<int>? UserIds { get; set; }
        public List<int>? CourseIds { get; set; }
    }
}
