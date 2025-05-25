using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Data.Data.Entities;

[Table("AuditLog", Schema = "logs")]
public class AuditLog
{
    [Key]
    public long Id { get; set; }
    [ForeignKey("AuditEntry")]
    public long AuditEntryId { get; set; }
    [MaxLength(50)] public string TableName { get; set; } = string.Empty;
    [MaxLength(10)] public string Action { get; set; } = string.Empty;
    [MaxLength(50)] public string ColumnName { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public virtual AuditEntry AuditEntry { get; set; }
}