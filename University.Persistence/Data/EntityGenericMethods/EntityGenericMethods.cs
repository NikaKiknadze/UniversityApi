using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace University.Data.Data.EntityGenericMethods;

public class EntityGenericMethods<TEntity> : IEntityGenericMethods<TEntity> where TEntity : class
{
    private readonly DbContext _context;

    public EntityGenericMethods(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IQueryable<TEntity> All => _context.Set<TEntity>();
    public IQueryable<TEntity> AllAsNoTracking => _context.Set<TEntity>().AsNoTracking();

    public virtual void Add(TEntity entity) => _context.Set<TEntity>().Add(entity);

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual void AddRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().AddRange(entities);

    public virtual void Remove(TEntity entity) => _context.Set<TEntity>().Remove(entity);

    public virtual void RemoveRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().RemoveRange(entities);

    public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task RemoveAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var entities = await _context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

        if (entities is { Count: > 0 })
        {
            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) => await _context.Set<TEntity>().AsNoTracking().AnyAsync(predicate);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().AddRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }
}