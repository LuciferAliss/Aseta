using System;
using Aseta.Domain.Abstractions.Persistence;

namespace Aseta.Infrastructure.Database;

internal sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}