using Microsoft.EntityFrameworkCore.Storage;

namespace University.Data.ContextMethodsDirectory;

public interface IContextMethods
{
    int Complete();
    Task<int> CompleteAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}