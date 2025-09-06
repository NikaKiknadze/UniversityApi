using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("UsersCourses", Schema = "university")]
public sealed class UserCourse
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public User User { get; set; }
    public Course Course { get; set; }
}