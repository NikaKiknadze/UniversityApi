using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("UserProfiles", Schema = "university")]
public class UserProfile
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    
    [MaxLength(50)]
    public required string FirstName { get; set; }
    
    [MaxLength(50)]
    public required string LastName { get; set; }
    
    public required int Age { get; set; }
    
    [ForeignKey("Faculty")]
    public int? FacultyId { get; set; }
    
    public virtual User? User { get; set; }
    public virtual Faculty? Faculty { get; init; }
}