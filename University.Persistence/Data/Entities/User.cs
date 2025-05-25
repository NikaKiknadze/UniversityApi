using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("Users", Schema = "ums")]
public class User
{
    [Key]
    public int Id { get; init; }
    [MaxLength(20)]
    public required string Username { get; init; }
    [MaxLength(70)]
    public required string PasswordHash { get; init; }
    public bool IsActive { get; set; } = true;
        
    public virtual ICollection<UsersLecturers> UsersLecturers { get; init; } = new HashSet<UsersLecturers>();
    public virtual ICollection<UsersCourses> UsersCourses { get; init; } = new HashSet<UsersCourses>();
    public virtual UserProfile? UserProfile { get; set; }
    public virtual ICollection<AuditEntry> AuditEntries { get; init; } = new HashSet<AuditEntry>();
}