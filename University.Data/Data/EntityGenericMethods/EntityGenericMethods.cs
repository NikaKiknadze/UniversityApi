using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace University.Data.Data.EntityGenericMethods;

public class EntityGenericMethods<TEntity>(DbContext context) : IEntityGenericMethods<TEntity>
    where TEntity : class
{
    public IQueryable<TEntity> All => context.Set<TEntity>();
    public IQueryable<TEntity> AllAsNoTracking => context.Set<TEntity>().AsNoTracking();

    public virtual void Add(TEntity entity) => context.Set<TEntity>().Add(entity);

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual void AddRange(IEnumerable<TEntity> entities) => context.Set<TEntity>().AddRange(entities);

    public virtual void Remove(TEntity entity) => context.Set<TEntity>().Remove(entity);

    public virtual void RemoveRange(IEnumerable<TEntity> entities) => context.Set<TEntity>().RemoveRange(entities);

    public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        context.Set<TEntity>().RemoveRange(entities);
        await context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task RemoveAllAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
    {
        var entities = await context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

        if (entities is { Count: > 0 })
        {
            context.Set<TEntity>().RemoveRange(entities);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) => await context.Set<TEntity>().AsNoTracking().AnyAsync(predicate);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        context.Set<TEntity>().AddRange(entities);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public int Complete()
    {
        return context.SaveChanges();
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}