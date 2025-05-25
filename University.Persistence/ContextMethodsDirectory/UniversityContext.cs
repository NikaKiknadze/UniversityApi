using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using University.Data.Data.Entities;
using University.Data.Data.EntityGenericMethods;

namespace University.Data.ContextMethodsDirectory;

public class UniversityContext : IUniversityContext
{
    private readonly UniversityDbContext _context;
    public UniversityContext(UniversityDbContext context)
    {
        _context = context;
        Database = context.Database;
        Courses = new ContextEntityGenericMethodsSetter<Course>(_context);
        Faculties = new ContextEntityGenericMethodsSetter<Faculty>(_context);
        Lecturers = new ContextEntityGenericMethodsSetter<Lecturer>(_context);
        Users = new ContextEntityGenericMethodsSetter<User>(_context);
        AuditLogs = new ContextEntityGenericMethodsSetter<AuditLog>(_context);
        AuditEntries = new ContextEntityGenericMethodsSetter<AuditEntry>(_context);
    }
    public DatabaseFacade Database { get; }

    #region MainMerhods

    public int Complete()
    {
        _context.TrackUntrackedEntities(_context);
        _context.ExecuteLogging();
        return _context.SaveChanges();
    }
    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        _context.TrackUntrackedEntities(_context);
        _context.ExecuteLogging();
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => await _context.Database.BeginTransactionAsync(cancellationToken);

    #endregion

    public IEntityGenericMethods<Course> Courses { get; }
    public IEntityGenericMethods<Faculty> Faculties { get; }
    public IEntityGenericMethods<Lecturer> Lecturers { get; }
    public IEntityGenericMethods<User> Users { get; }
    public IEntityGenericMethods<AuditLog> AuditLogs { get; }
    public IEntityGenericMethods<AuditEntry> AuditEntries { get; }
}