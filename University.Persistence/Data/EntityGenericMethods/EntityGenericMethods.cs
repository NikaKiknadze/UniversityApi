using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace University.Data.Data.EntityGenericMethods;

public class EntityGenericMethods<TEntity>(DbContext context) : IEntityGenericMethods<TEntity>
    where TEntity : class
{
    private readonly DbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IQueryable<TEntity> All => _context.Set<TEntity>();
    public IQueryable<TEntity> AllAsNoTracking => _context.Set<TEntity>().AsNoTracking();

    public virtual void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public virtual void AddRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().AddRange(entities);

    public virtual void Remove(TEntity entity) => _context.Set<TEntity>().Remove(entity);

    public virtual void RemoveRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().RemoveRange(entities);
    public virtual void RemoveAll(Expression<Func<TEntity, bool>> predicate)
    {
        var entities =  _context.Set<TEntity>().AsNoTracking().Where(predicate).ToList();

        if (entities is { Count: > 0 })
            _context.Set<TEntity>().RemoveRange(entities);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) => await _context.Set<TEntity>().AsNoTracking().AnyAsync(predicate);
}