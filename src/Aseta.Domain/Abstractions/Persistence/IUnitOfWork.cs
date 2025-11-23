namespace Aseta.Domain.Abstractions.Persistence;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<ITransactionScope> BeginTransactionScopeAsync(CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
