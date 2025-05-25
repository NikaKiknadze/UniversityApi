using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("AuditEntries", Schema = "logs")]
public class AuditEntry
{
    [Key]
    public long Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    [MaxLength(100)]
    public string EntityType { get; set; } = string.Empty;
    [MaxLength(100)]
    public string TableName { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;
    [MaxLength(100)]
    public string PrimaryKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual List<AuditLog> Logs { get; set; } = [];
    public virtual User? User { get; set; }
}