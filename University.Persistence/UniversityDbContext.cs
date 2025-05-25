using Microsoft.EntityFrameworkCore;
using University.Data.Data.Entities;
using University.Data.LogHelpers;

namespace University.Data;

public class UniversityDbContext(
    DbContextOptions<UniversityDbContext> options,
    IHttpContextAccessor httpContextAccessor) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Faculty> Faculty { get; set; }
    public DbSet<UsersCourses> UsersCoursesJoin { get; set; }
    public DbSet<UsersLecturers> UsersLecturersJoin { get; set; }
    public DbSet<CoursesLecturersJoin> CoursesLecturersJoin { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<AuditEntry> AuditEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsersCourses>()
            .HasKey(n => new { n.UserId, n.CourseId });
        modelBuilder.Entity<UsersCourses>()
            .HasOne(u => u.User)
            .WithMany(k => k.UsersCourses)
            .HasForeignKey(i => i.UserId);
        modelBuilder.Entity<UsersCourses>()
            .HasOne(c => c.Course)
            .WithMany(k => k.UsersCourses)
            .HasForeignKey(u => u.CourseId);


        modelBuilder.Entity<UsersLecturers>()
            .HasKey(n => new { n.UserId, n.LecturerId });
        modelBuilder.Entity<UsersLecturers>()
            .HasOne(u => u.User)
            .WithMany(k => k.UsersLecturers)
            .HasForeignKey(u => u.UserId);
        modelBuilder.Entity<UsersLecturers>()
            .HasOne(l => l.Lecturer)
            .WithMany(k => k.UsersLecturers)
            .HasForeignKey(l => l.LecturerId);


        modelBuilder.Entity<CoursesLecturersJoin>()
            .HasKey(n => new { n.CourseId, n.LectureId });
        modelBuilder.Entity<CoursesLecturersJoin>()
            .HasOne(c => c.Course)
            .WithMany(k => k.CoursesLecturers)
            .HasForeignKey(c => c.CourseId);
        modelBuilder.Entity<CoursesLecturersJoin>()
            .HasOne(l => l.Lecturer)
            .WithMany(k => k.CoursesLecturers)
            .HasForeignKey(l => l.LectureId);


        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    #region Logging

    public void ExecuteLogging()
    {
        var auditEntries = new List<AuditEntry>();
        var userId = httpContextAccessor.HttpContext?.Session.GetInt32("UserId") ?? 0;

        var allEntries = ChangeTracker.Entries().ToList();
        var discovered = new HashSet<object>();

        foreach (var navEntry in allEntries.ToList().Where(entry => discovered.Add(entry.Entity))
                     .Select(entry => entry.References
                     .Where(r => r.TargetEntry != null)
                     .Select(r => r.TargetEntry)
                     .Concat(entry.Collections
                         .SelectMany(c => c.CurrentValue as IEnumerable<object> ?? [])
                         .Select(Entry))
                     .Where(e => e != null && !allEntries.Contains(e))
                     .ToList()).SelectMany(navProps => navProps))
        {
            allEntries.Add(navEntry!);
            discovered.Add(navEntry!.Entity);
        }

        foreach (var entry in allEntries
                     .Where(e => e.Entity is not AuditEntry &&
                                 e.State != EntityState.Detached &&
                                 e.State != EntityState.Unchanged))
        {
            var auditEntry = new AuditEntry
            {
                UserId = userId,
                EntityType = entry.Entity.GetType().Name,
                TableName = entry.Metadata.GetTableName() ?? string.Empty,
                Action = entry.State.ToString(),
                PrimaryKey = entry.GetPrimaryKeyValue(),
                CreatedAt = DateTime.UtcNow
            };

            foreach (var prop in entry.Properties)
            {
                if (prop.IsTemporary) continue;

                string? originalValue = null;
                string? currentValue = null;
                var action = string.Empty;

                if (entry.State == EntityState.Added)
                {
                    action = "Add";
                    currentValue = prop.CurrentValue?.ToString();
                }
                else if (entry.State == EntityState.Deleted)
                {
                    action = "Delete";
                    originalValue = prop.OriginalValue?.ToString();
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (!Equals(prop.OriginalValue, prop.CurrentValue))
                    {
                        action = "Update";
                        originalValue = prop.OriginalValue?.ToString();
                        currentValue = prop.CurrentValue?.ToString();
                    }
                }

                if (originalValue != null || currentValue != null)
                {
                    auditEntry.Logs.Add(new AuditLog
                    {
                        AuditEntryId = auditEntry.Id,
                        TableName = entry.Metadata.GetTableName() ?? string.Empty,
                        Action = action.GetActionType(prop.Metadata.Name, originalValue),
                        ColumnName = prop.Metadata.Name,
                        OldValue = originalValue,
                        NewValue = currentValue
                    });
                }
            }

            if (auditEntry.Logs.Count != 0)
                auditEntries.Add(auditEntry);
        }

        AuditEntries.AddRange(auditEntries);
    }
    
    public void TrackUntrackedEntities(DbContext context)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State != EntityState.Detached && e.State != EntityState.Unchanged)
            .ToList();

        foreach (var entry in entries)
        {
            foreach (var navigation in entry.Navigations)
            {
                if (!navigation.IsLoaded) continue;

                if (navigation.CurrentValue is IEnumerable<object> collection)
                {
                    foreach (var item in collection)
                    {
                        var itemEntry = context.Entry(item);
                        if (itemEntry.State == EntityState.Detached)
                            context.Add(item); // Force EF to track it
                    }
                }
                else if (navigation.CurrentValue != null)
                {
                    var itemEntry = context.Entry(navigation.CurrentValue);
                    if (itemEntry.State == EntityState.Detached)
                        context.Add(navigation.CurrentValue); // Force EF to track it
                }
            }
        }
    }

    #endregion
}