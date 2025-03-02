namespace University.Domain.Models
{
    public class FacultyPutDto(int id, string facultyName, List<int>? userIds, List<int>? courseIds)
    {
        public int Id { get; } = id;
        public required string FacultyName { get; init; } = facultyName;
        public List<int>? UserIds { get; } = userIds;
        public List<int>? CourseIds { get; } = courseIds;
    }
}
