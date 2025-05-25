using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("UsersLecturersJoin", Schema = "university")]
public class UsersLecturers
{
    public int UserId { get; init; }
    public int LecturerId { get; init; }
    public virtual User? User { get; init; }
    public virtual Lecturer? Lecturer { get; init; }
}