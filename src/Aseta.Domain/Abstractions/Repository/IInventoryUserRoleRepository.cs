using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Abstractions.Repository;

public interface IInventoryUserRoleRepository : IRepository<InventoryUserRole, Guid>
{
    Task<InventoryUserRole?> GetUserGrantToInventoryAsync(Guid userId, Guid inventoryId, InventoryRole role);
    Task<bool> UserHasRoleAsync(Guid userId, Guid inventoryId, InventoryRole role);
}
