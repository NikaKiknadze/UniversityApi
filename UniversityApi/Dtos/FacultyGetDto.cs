﻿using UniversityApi.Entities;

namespace UniversityApi.Dtos
{
    public class FacultyGetDto
    {
        public int? Id { get; set; }

        public string? FacultyName { get; set; }

        public List<UserOnlyDto>? Users { get; set; }

        public List<CourseOnlyDto>? Courses { get; set; }
    }
}
