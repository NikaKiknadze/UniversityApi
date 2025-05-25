using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("Courses", Schema = "university")]
public class Course
{
    [Key]
    public int Id { get; init; }
    [MaxLength(50)]
    public required string CourseName { get; set; }
    [ForeignKey("Faculty")]
    public int? FacultyId { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual ICollection<CoursesLecturersJoin> CoursesLecturers { get; init; } = new HashSet<CoursesLecturersJoin>();
    public virtual ICollection<UsersCourses> UsersCourses { get; init; } = new HashSet<UsersCourses>();
    public virtual Faculty? Faculty { get; set; }
}