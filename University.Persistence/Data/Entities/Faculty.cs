using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities
{
    [Table("Faculties", Schema = "university")]
    public class  Faculty
    {
        [Key]
        public int Id { get; init; }
        [MaxLength(50)]
        public required string FacultyName { get; set; }

        public bool IsActive { get; set; } = true;
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    }
}
