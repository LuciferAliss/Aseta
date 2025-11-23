using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.UserRoles;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Users;

internal sealed class InventoryUserRoleRepository(AppDbContext context) : Repository<InventoryRole>(context), IInventoryUserRoleRepository
{
    public Task<Role> GetUserRoleInInventory(string userId, Guid inventoryI, CancellationToken cancellationToken = default)
    {
        return _dbSet.Where(i => i.UserId == Guid.Parse(userId) 
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