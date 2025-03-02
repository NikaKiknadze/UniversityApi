namespace University.Domain.Models.LecturerModels
{
    public class LecturerGetFilter : Paging
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public int? Age { get; set; }
        public bool IsActive { get; set; } = true;
        public List<int>? UserIds { get; set; }
        public List<int>? CourseIds { get; set; }
    }
}
