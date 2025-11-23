namespace Aseta.Domain.Abstractions.Persistence;

public interface ITransactionScope : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
