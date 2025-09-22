using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Abstractions.Repository;

public interface IInventoryUserRoleRepository : IRepository<InventoryUserRole, Guid>
{
    Task<InventoryUserRole?> GetUserGrantToInventoryAsync(Guid userId, Guid inventoryId, InventoryRoleType role);
    Task<bool> UserHasRoleAsync(Guid userId, Guid inventoryId, InventoryRoleType role);
    Task<InventoryUserRole?> GetUserRoleInventoryAsync(Guid userId, Guid inventoryId);
}
