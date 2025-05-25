using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace University.Data.LogHelpers;

public static class PrimaryKeyValueGetter
{
    public static string GetPrimaryKeyValue(this EntityEntry entry)
    {
        var keyNames = entry.Metadata.FindPrimaryKey()?.Properties.Select(p => p.Name).ToList();

        if (keyNames == null || keyNames.Count == 0)
            return string.Empty;

        var keyValues = keyNames.Select(name =>
            entry.Property(name).CurrentValue?.ToString()
            ?? entry.Property(name).OriginalValue?.ToString()
            ?? ""
        ).ToArray();

        return string.Join(",", keyValues);
    }
}