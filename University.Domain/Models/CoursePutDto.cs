namespace University.Domain.Models
{
    public class CoursePutDto(List<int>? lecturerIds, List<int>? userIds, string courseName, int? facultyId, int id)
    {
        public int Id { get; } = id;
        public required string CourseName { get; init; } = courseName;
        public int? FacultyId { get; } = facultyId;
        public List<int>? LecturerIds { get; } = lecturerIds;
        public List<int>? UserIds { get; } = userIds;
    }
}
