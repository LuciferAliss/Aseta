using System;
using Aseta.Domain.Abstractions;

namespace Aseta.Infrastructure.Database;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync();
    }
}
