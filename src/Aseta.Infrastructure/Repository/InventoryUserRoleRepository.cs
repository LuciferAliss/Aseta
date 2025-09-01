using System;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class InventoryUserRoleRepository(AppDbContext context) : Repository<InventoryUserRole>(context), IInventoryUserRoleRepository
{
    public async Task<bool> UserHasRoleAsync(Guid userId, Guid inventoryId, InventoryRole role)
    {
        return await _dbSet.AnyAsync(iur => iur.UserId == userId && iur.InventoryId == inventoryId && (iur.Role == role || iur.Role == InventoryRole.Owner));
    }
}
