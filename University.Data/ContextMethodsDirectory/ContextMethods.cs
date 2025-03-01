using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using University.Data.Data.Entities;
using University.Data.Data.EntityGenericMethods;

namespace University.Data.ContextMethodsDirectory;

public class ContextMethods : IContextMethods
{
    private readonly UniversityContext _context;
    public DatabaseFacade Database { get; }

    public ContextMethods(UniversityContext context)
    {
        _context = context;
        Database = context.Database;
        Courses = new ContextEntityGenericMethodsSetter<Course>(_context);
        Faculties = new ContextEntityGenericMethodsSetter<Faculty>(_context);
        Lecturers = new ContextEntityGenericMethodsSetter<Lecturer>(_context);
        Users = new ContextEntityGenericMethodsSetter<User>(_context);
    }

    #region EntityGenericMethods

    public IEntityGenericMethods<Course> Courses { get; }
    public IEntityGenericMethods<Faculty> Faculties { get; }
    public IEntityGenericMethods<Lecturer> Lecturers { get; }
    public IEntityGenericMethods<User> Users { get; }

    #endregion

    #region ContextMethods

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }
    
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}