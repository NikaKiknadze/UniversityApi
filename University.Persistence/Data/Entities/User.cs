using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities
{
    [Table("Users", Schema = "university")]
    public class User
    {
        [Key]
        public int Id { get; init; }
        [MaxLength(20)]
        public required string Name { get; set; }
        [MaxLength(20)]
        public required string SurName { get; set; }
        public required int Age { get; set; }
        [ForeignKey("Faculty")]
        public int? FacultyId { get; set; }

        public bool IsActive { get; set; } = true;
        public virtual ICollection<UsersLecturers> UsersLecturers { get; init; } = new HashSet<UsersLecturers>();
        public virtual ICollection<UsersCourses> UsersCourses { get; init; } = new HashSet<UsersCourses>();
        public virtual Faculty? Faculty { get; init; }
    }
}
