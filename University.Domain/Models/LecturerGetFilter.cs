namespace University.Domain.Models
{
    public class LecturerGetFilter(
        int? id,
        string? name,
        string? surName,
        int? age,
        List<int>? userIds,
        List<int>? courseIds)
        : Paging
    {
        public int? Id { get; } = id;
        public string? Name { get; } = name;
        public string? SurName { get; } = surName;
        public int? Age { get; } = age;
        public static bool IsActive => true;
        public List<int>? UserIds { get; } = userIds;
        public List<int>? CourseIds { get; } = courseIds;
    }
}
