using System;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class InventoryUserRoleRepository(AppDbContext context) : Repository<InventoryUserRole, Guid>(context), IInventoryUserRoleRepository
{
    public async Task<InventoryUserRole?> GetUserGrantToInventoryAsync(Guid userId, Guid inventoryId, InventoryRole role)
    {
        return await _dbSet.FirstOrDefaultAsync(iur => iur.InventoryId == inventoryId && iur.UserId == userId && iur.Role == role);
    }

    public async Task<InventoryUserRole?> GetUserRoleInventoryAsync(Guid inventoryId, Guid userId)
    {
        return await _dbSet.FirstOrDefaultAsync(iur => iur.UserId == userId && iur.InventoryId == inventoryId);
    }

    public async Task<bool> UserHasRoleAsync(Guid userId, Guid inventoryId, InventoryRole role)
    {
        return await _dbSet.AnyAsync(iur => iur.UserId == userId && iur.InventoryId == inventoryId && (iur.Role == role || iur.Role == InventoryRole.Owner));
    }
}
