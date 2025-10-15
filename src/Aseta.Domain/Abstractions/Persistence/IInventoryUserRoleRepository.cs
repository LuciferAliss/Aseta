using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Enums;

namespace Aseta.Domain.Abstractions.Persistence;

public interface IInventoryUserRoleRepository : IRepository<InventoryUserRole>
{
    Task<bool> UserHasRoleAsync(
        Guid userId,
        Guid inventoryId,
        Role role,
        CancellationToken cancellationToken = default);
}