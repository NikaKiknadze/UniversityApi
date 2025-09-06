using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("UsersLecturers", Schema = "university")]
public sealed class UserLecturer
{
    public int UserId { get; set; }
    public int LecturerId { get; set; }
    public User User { get; set; }
    public Lecturer Lecturer { get; set; }
}