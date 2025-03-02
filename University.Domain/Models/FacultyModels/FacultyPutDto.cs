namespace University.Domain.Models.FacultyModels
{
    public class FacultyPutDto
    {
        public int Id { get; set; }
        public string FacultyName { get; set; } = null!;
        public List<int>? UserIds { get; set; }
        public List<int>? CourseIds { get; set; }
    }
}
