using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities
{
    [Table("UsersCoursesJoin", Schema = "university")]
    public class UsersCoursesJoin
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}
