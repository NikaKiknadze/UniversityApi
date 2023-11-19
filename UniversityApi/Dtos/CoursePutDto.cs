using UniversityApi.Entities;

namespace UniversityApi.Dtos
{
    public class CoursePutDto
    {
        public int? Id { get; set; }

        public string? CourseName { get; set; }

        public int? FacultyId { get; set; }

        public List<int>? LecturerIds { get; set; }

        public List<int>? UserIds { get; set; }
    }
}
