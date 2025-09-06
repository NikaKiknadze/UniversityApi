namespace University.Data.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> All { get; }
    IQueryable<T> AllAsNoTracking { get; }
    void Add(T entity);
    void Remove(T entity);
    void Update(T entity);
    void Complete(T entity);
    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task RemoveAsync(int id, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task CompleteAsync(CancellationToken cancellationToken);
}