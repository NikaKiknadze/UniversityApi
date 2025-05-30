﻿using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("CoursesLecturersJoin", Schema = "university")]
public class CoursesLecturersJoin
{
    public int CourseId { get; set; }
    public int LectureId { get; set; }
    public virtual Course? Course { get; set; }
    public virtual Lecturer? Lecturer { get; set; }
}