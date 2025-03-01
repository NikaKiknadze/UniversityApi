using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities
{
    [Table("Courses", Schema = "university")]
    public class Course
    {
        [Key]
        public int Id { get; init; }
        [MaxLength(50)]
        public required string CourseName { get; set; }
        [ForeignKey("Faculty")]
        public int? FacultyId { get; set; }
        
        public virtual ICollection<CoursesLecturersJoin> CoursesLecturers { get; set; } = new HashSet<CoursesLecturersJoin>();
        public virtual ICollection<UsersCoursesJoin> UsersCourses { get; set; } = new HashSet<UsersCoursesJoin>();
        public virtual Faculty? Faculty { get; init; }
    }
}
