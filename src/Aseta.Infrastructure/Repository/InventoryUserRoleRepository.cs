using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Enums;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class InventoryUserRoleRepository(AppDbContext context) : Repository<InventoryRole>(context), IInventoryUserRoleRepository
{
    public async Task<bool> HasUserRoleAsync(
        string userId,
        Guid inventoryId,
        Role role,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(iur =>
            iur.UserId == Guid.Parse(userId) &&
            iur.InventoryId == inventoryId &&
            (iur.Role == role || iur.Role == Role.Owner),
            cancellationToken);
    }
}