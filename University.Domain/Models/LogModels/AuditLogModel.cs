namespace University.Domain.Models.LogModels;

public class AuditLogModel
{
    public DateTime LogDate { get; set; }
    public string UserFirstName { get; set; } = string.Empty;
    public string UserLastName { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}