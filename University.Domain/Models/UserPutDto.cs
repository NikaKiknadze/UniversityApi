namespace University.Domain.Models
{
    public class UserPutDto(
        int id,
        string name,
        string surname,
        int age,
        int? facultyId,
        List<int>? lecturerIds,
        List<int>? courseIds)
    {
        public int Id { get; } = id;
        public required string Name { get; init; } = name;
        public required string Surname { get; init; } = surname;
        public required int Age { get; init; } = age;
        public int? FacultyId { get; } = facultyId;
        public List<int>? LecturerIds { get; } = lecturerIds;
        public List<int>? CourseIds { get; } = courseIds;
    }
}
