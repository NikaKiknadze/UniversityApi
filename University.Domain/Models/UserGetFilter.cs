namespace University.Domain.Models
{
    public class UserGetFilter(
        int? id,
        string? name,
        string? surName,
        int? age,
        int? facultyId,
        List<int>? courseIds,
        List<int>? lecturerIds)
        : Paging
    {
        public int? Id { get; } = id;
        public string? Name { get; } = name;
        public string? SurName { get; } = surName;
        public int? Age { get; } = age;
        public int? FacultyId { get; } = facultyId;
        public static bool IsActive => true;
        public List<int>? CourseIds { get; } = courseIds;
        public List<int>? LecturerIds { get; } = lecturerIds;
    }
}
