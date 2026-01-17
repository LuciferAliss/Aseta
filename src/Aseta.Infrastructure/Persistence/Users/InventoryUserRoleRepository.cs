using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Users;

public sealed class InventoryUserRoleRepository(AppDbContext context) : Repository<InventoryRole>(context), IInventoryUserRoleRepository
{
    public Task<InventoryRole?> GetUserRoleInInventory(Guid userId, Guid inventoryI, CancellationToken cancellationToken = default)
    {
        return _dbSet.Where(i => i.UserId == userId &&
            i.InventoryId == inventoryI)
            .FirstOrDefaultAsync(cancellationToken);
    }
}