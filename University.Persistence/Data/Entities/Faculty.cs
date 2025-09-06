using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("Faculties", Schema = "university")]
public sealed class  Faculty
{
    [Key]
    public int Id { get; set; }
    [MaxLength(50)]
    public required string FacultyName { get; set; }

    public bool IsActive { get; set; } = true;
    public ICollection<UserProfile> Users { get; set; } = new HashSet<UserProfile>();
    public ICollection<FacultyCourse> FacultyCourses { get; set; } = new HashSet<FacultyCourse>();
}