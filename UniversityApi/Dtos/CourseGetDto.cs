using UniversityApi.Entities;

namespace UniversityApi.Dtos
{
    public class CourseGetDto
    {
        public int? Id { get; set; }

        public string? CourseName { get; set; }

        public FacultyOnlyDto? Faculty { get; set; }

        public List<LecturerOnlyDto>? Lecturers { get; set; }

        public List<UserOnlyDto>? Users { get; set; }
    }
}
