using UniversityApi.Entities;

namespace UniversityApi.Dtos
{
    public class CourseGetDto
    {
        public int? Id { get; set; }

        public string? CourseName { get; set; }

        public Faculty? Faculty { get; set; }

        public List<int>? LecturerIds { get; set; }

        public List<int>? UserIds { get; set; }
    }
}
