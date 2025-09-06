using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("Courses", Schema = "university")]
public sealed class Course
{
    [Key]
    public int Id { get; set; }
    [MaxLength(50)]
    public required string CourseName { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<CourseLecturer> CoursesLecturers { get; set; } = new List<CourseLecturer>();
    public ICollection<UserCourse> UsersCourses { get; set; } = new List<UserCourse>();
    public ICollection<FacultyCourse> FacultyCourses { get; set; } = new List<FacultyCourse>();
}