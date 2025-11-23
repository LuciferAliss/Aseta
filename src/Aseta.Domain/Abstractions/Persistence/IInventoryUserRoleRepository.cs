using Aseta.Domain.Entities.UserRoles;

namespace Aseta.Domain.Abstractions.Persistence;

public interface IInventoryUserRoleRepository : IRepository<InventoryRole>
{
    Task<Role> GetUserRoleInInventory(string userId, Guid inventoryI, CancellationToken cancellationToken = default);
}