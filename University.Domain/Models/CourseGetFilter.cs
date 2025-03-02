namespace University.Domain.Models
{
    public class CourseGetFilter(
        List<int>? lecturerIds,
        List<int>? userIds,
        int? facultyId,
        string? courseName,
        int? id)
        : Paging
    {
        public int? Id = id;
        public static bool IsActive => true;
        public readonly string? CourseName = courseName;

        public int? FacultyId = facultyId;

        public readonly List<int>? LecturerIds = lecturerIds;
        public readonly List<int>? UserIds = userIds;
    }
}
