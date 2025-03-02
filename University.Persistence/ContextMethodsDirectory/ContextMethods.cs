using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using University.Data.Data.Entities;
using University.Data.Data.EntityGenericMethods;

namespace University.Data.ContextMethodsDirectory;

public class ContextMethods : IContextMethods
{
    private readonly UniversityDbContext _dbContext;
    public DatabaseFacade Database { get; }

    public ContextMethods(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
        Database = dbContext.Database;
        Courses = new ContextEntityGenericMethodsSetter<Course>(_dbContext);
        Faculties = new ContextEntityGenericMethodsSetter<Faculty>(_dbContext);
        Lecturers = new ContextEntityGenericMethodsSetter<Lecturer>(_dbContext);
        Users = new ContextEntityGenericMethodsSetter<User>(_dbContext);
    }

    #region EntityGenericMethods
    public IEntityGenericMethods<Course> Courses { get; }
    public IEntityGenericMethods<Faculty> Faculties { get; }
    public IEntityGenericMethods<Lecturer> Lecturers { get; }
    public IEntityGenericMethods<User> Users { get; }
    #endregion

    #region ContextMethods
    public int Complete() => _dbContext.SaveChanges();
    public async Task<int> CompleteAsync(CancellationToken cancellationToken) =>  await _dbContext.SaveChangesAsync(cancellationToken);
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => await _dbContext.Database.BeginTransactionAsync(cancellationToken);

    #endregion
}