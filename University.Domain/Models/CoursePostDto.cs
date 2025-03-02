namespace University.Domain.Models
{
    public class CoursePostDto(List<int>? lecturerIds, List<int>? userIds, int? facultyId, string courseName)
    {
        public required string CourseName { get; init; } = courseName;
        public int? FacultyId { get; } = facultyId;
        public List<int>? LecturerIds { get; } = lecturerIds;
        public List<int>? UserIds { get; } = userIds;
    }
}
