using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities
{
    [Table("Lecturers",Schema = "university")]
    public class Lecturer
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string SurName { get; set; }
        public int Age { get; set; }

        public virtual ICollection<UsersLecturersJoin>? UsersLecturers { get; set; }
        public virtual ICollection<CoursesLecturersJoin>? CoursesLecturers { get; set; }

    }
}
