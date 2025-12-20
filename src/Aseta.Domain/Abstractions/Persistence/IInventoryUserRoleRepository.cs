using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Domain.Abstractions.Persistence;

public interface IInventoryUserRoleRepository : IRepository<InventoryRole>
{
    Task<Role> GetUserRoleInInventory(Guid userId, Guid inventoryI, CancellationToken cancellationToken = default);
}