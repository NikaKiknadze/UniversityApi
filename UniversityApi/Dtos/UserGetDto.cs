namespace UniversityApi.Dtos
{
    public class UserGetDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }
        
        public string? Surname { get; set; }
        
        public int? Age { get; set; }
        
        public FacultyGetDto? Faculty { get; set; }
        
        public List<int>? LecturerIds { get; set; }
        
        public List<int>? CourseIds { get; set; }
    }
}
