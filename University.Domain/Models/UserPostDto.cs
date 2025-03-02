namespace University.Domain.Models
{
    public class UserPostDto(
        string name,
        string surName,
        int age,
        int? facultyId,
        List<int>? courseIds,
        List<int>? lecturerIds)
    {
        public required string Name { get; init; } = name;
        public required string SurName { get; init; } = surName;
        public required int Age { get; init; } = age;
        public int? FacultyId { get; } = facultyId;
        public List<int>? CourseIds { get; } = courseIds;
        public List<int>? LecturerIds { get; } = lecturerIds;
    }
}
