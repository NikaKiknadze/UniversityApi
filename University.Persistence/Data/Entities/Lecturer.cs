using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("Lecturers",Schema = "ums")]
public sealed class Lecturer
{
    [Key]
    public int Id { get; set; }
    [MaxLength(20)]
    public required string Name { get; set; }
    [MaxLength(20)]
    public required string SurName { get; set; }
    public required int Age { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<UserLecturer> UsersLecturers { get; set; } = new HashSet<UserLecturer>();
    public ICollection<CourseLecturer> CoursesLecturers { get; set; } = new HashSet<CourseLecturer>();

}