namespace University.Domain.Models
{
    public class UserGetDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }
        
        public string? SurName { get; set; }
        
        public int? Age { get; set; }
        
        public FacultyOnlyDto? Faculty { get; set; }
        
        public List<LecturerOnlyDto>? Lecturers { get; set; }
        
        public List<CourseOnlyDto>? Courses { get; set; }
    }
}
