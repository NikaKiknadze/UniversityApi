using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("CoursesLecturers", Schema = "university")]
public sealed class CourseLecturer
{
    public int CourseId { get; set; }
    public int LectureId { get; set; }
    public Course Course { get; set; }
    public Lecturer Lecturer { get; set; }
}