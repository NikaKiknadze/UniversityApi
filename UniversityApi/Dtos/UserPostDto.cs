﻿namespace UniversityApi.Dtos
{
    public class UserPostDto
    {
        public string? Name { get; set; }

        public string? SurName { get; set; }

        public int? Age { get; set; }

        public int? FacultyId { get; set; }

        public List<int>? CourseIds { get; set; }

        public List<int>? LecturerIds { get; set; }
    }
}
