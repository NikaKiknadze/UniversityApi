using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("Users", Schema = "ums")]
public sealed class User
{
    [Key]
    public int Id { get; set; }
    [MaxLength(20)]
    public required string Username { get; set; }
    [MaxLength(70)]
    public required string PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
        
    public ICollection<UserLecturer> UsersLecturers { get; set; } = new HashSet<UserLecturer>();
    public ICollection<UserCourse> UsersCourses { get; set; } = new HashSet<UserCourse>();
    public UserProfile UserProfile { get; set; }
}