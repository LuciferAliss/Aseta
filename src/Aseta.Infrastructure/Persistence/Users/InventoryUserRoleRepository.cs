using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Users;

public sealed class InventoryUserRoleRepository(AppDbContext context) : Repository<InventoryRole>(context), IInventoryUserRoleRepository
{
    public Task<Role> GetUserRoleInInventory(Guid userId, Guid inventoryI, CancellationToken cancellationToken = default)
    {
        return _dbSet.Where(i => i.UserId == userId
            && i.InventoryId == inventoryI)
            .Select(i => i.Role)
            .FirstOrDefaultAsync(cancellationToken);
    }

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