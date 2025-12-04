using Aseta.Domain.Abstractions.Persistence;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace Aseta.Infrastructure.Persistence.Common;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    private sealed class TransactionScope(UnitOfWork unitOfWork) : ITransactionScope
    {
        private readonly UnitOfWork _unitOfWork = unitOfWork;
        private bool _isCommitted;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            _isCommitted = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (!_isCommitted)
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
        }
    }

    public async Task<ITransactionScope> BeginTransactionScopeAsync(CancellationToken cancellationToken = default)
    {
        await BeginTransactionAsync(cancellationToken);
        return new TransactionScope(this);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.DisposeAsync();
        }
    }

    private async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is not null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _currentTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    private async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("Transaction has not been started.");
        }

        try
        {
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    private async Task RollbackTransactionAsync()
    {
        if (_currentTransaction is null)
        {
            return;
        }

        try
        {
            await _currentTransaction.RollbackAsync();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}