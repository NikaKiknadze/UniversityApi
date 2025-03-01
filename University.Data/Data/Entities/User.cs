using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities
{
    [Table("Users", Schema = "university")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string SurName { get; set; }
        public int Age { get; set; }
        [ForeignKey("Faculty")]
        public int? FacultyId { get; set; }

        public virtual ICollection<UsersLecturersJoin>? UsersLecturers { get; set; }
        public virtual ICollection<UsersCoursesJoin>? UsersCourses { get; set; }
        public virtual Faculty? Faculty { get; set; }
    }
}
