using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities
{
    [Table("Lecturers",Schema = "university")]
    public class Lecturer
    {
        [Key]
        public int Id { get; init; }
        [MaxLength(20)]
        public required string Name { get; set; }
        [MaxLength(20)]
        public required string SurName { get; set; }
        public required int Age { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<UsersLecturers> UsersLecturers { get; init; } = new HashSet<UsersLecturers>();
        public virtual ICollection<CoursesLecturersJoin> CoursesLecturers { get; init; } = new HashSet<CoursesLecturersJoin>();

    }
}
