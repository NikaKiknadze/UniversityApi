namespace University.Domain.Models
{
    public class FacultyGetFilter(string? facultyName, List<int>? userIds, List<int>? courseIds, int? id)
        : Paging
    {
        public int? Id { get; } = id;
        public static bool IsActive => true;
        public string? FacultyName { get; } = facultyName;
        public List<int>? UserIds { get; } = userIds;
        public List<int>? CourseIds { get; } = courseIds;
    }
}
