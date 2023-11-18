using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApi.Entities
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

        public virtual ICollection<UsersLecturersJoin>? UsersLecturers { get; set; }
        public virtual ICollection<UsersCoursesJoin>? UsersCourses { get; set; }
        public virtual Faculty? Faculty { get; set; }
    }
}
