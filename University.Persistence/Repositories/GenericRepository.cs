using Microsoft.EntityFrameworkCore;
using University.Data.Data;
using University.Data.Repositories.Interfaces;
using University.Domain.CustomExceptions;

namespace University.Data.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public IQueryable<T> All => context.Set<T>();
    public IQueryable<T> AllAsNoTracking => context.Set<T>().AsNoTracking();
    public void Add(T entity) => _dbSet.Add(entity);

    public void Remove(T entity) => _dbSet.Remove(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Complete(T entity) => context.SaveChanges();

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _dbSet.FindAsync([id], cancellationToken) ?? throw new NotFoundException("Record not found!");

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        _dbSet.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task CompleteAsync(CancellationToken cancellationToken) =>
        await context.SaveChangesAsync(cancellationToken);
}