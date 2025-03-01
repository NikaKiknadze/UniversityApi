using System.Linq.Expressions;

namespace University.Data.Data.EntityGenericMethods;

public interface IEntityGenericMethods<TEntity> where TEntity : class
{
    IQueryable<TEntity> All { get; }
    IQueryable<TEntity> AllAsNoTracking { get; }
    void Add(TEntity entity);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    void AddRange(IEnumerable<TEntity> entities);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task RemoveAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    int Complete();
    Task<int> CompleteAsync(CancellationToken cancellationToken);
}