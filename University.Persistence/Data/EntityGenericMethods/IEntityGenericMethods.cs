using System.Linq.Expressions;

namespace University.Data.Data.EntityGenericMethods;

public interface IEntityGenericMethods<TEntity> where TEntity : class
{
    IQueryable<TEntity> All { get; }
    IQueryable<TEntity> AllAsNoTracking { get; }
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    void RemoveAll(Expression<Func<TEntity, bool>> predicate);
}