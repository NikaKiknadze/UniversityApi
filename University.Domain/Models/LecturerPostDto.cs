namespace University.Domain.Models
{
    public class LecturerPostDto(string name, string surname, int age, List<int>? userIds, List<int>? courseIds)
    {
        public required string Name { get; init; } = name;
        public required string Surname { get; init; } = surname;
        public required int Age { get; init; } = age;
        public List<int>? UserIds { get; } = userIds;
        public List<int>? CourseIds { get; } = courseIds;
    }
}
