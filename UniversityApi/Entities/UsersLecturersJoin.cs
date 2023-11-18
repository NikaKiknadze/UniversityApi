using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApi.Entities
{
    [Table("UsersLecturersJoin", Schema = "university")]
    public class UsersLecturersJoin
    {
        public int UserId { get; set; }
        public int LecturerId { get; set; }
        public virtual User User { get; set; }
        public virtual Lecturer Lecturer { get; set; }
    }
}
