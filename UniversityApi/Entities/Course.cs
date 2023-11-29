using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApi.Entities
{
    [Table("Courses", Schema = "university")]
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string CourseName { get; set; }
        [ForeignKey("Faculty")]
        public int? FacultyId { get; set; }
        
        public virtual ICollection<CoursesLecturersJoin>? CoursesLecturers { get; set; }
        public virtual ICollection<UsersCoursesJoin>? UsersCourses { get; set; }
        public virtual Faculty? Faculty { get; set; }
    }
}
