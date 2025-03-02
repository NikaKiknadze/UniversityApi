namespace University.Domain.Models
{
    public class FacultyPostDto(string facultyName, List<int>? userIds, List<int>? courseIds)
    {
        public required string FacultyName { get; init; } = facultyName;
        public List<int>? UserIds { get; } = userIds;
        public List<int>? CourseIds { get; } = courseIds;
    }
}
