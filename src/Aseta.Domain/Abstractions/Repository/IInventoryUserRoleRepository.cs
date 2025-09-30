using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Abstractions.Repository;

public interface IInventoryUserRoleRepository : IRepository<InventoryUserRole>
{
    Task<InventoryUserRole?> GetUserGrantToInventoryAsync(
        Guid userId,
        Guid inventoryId,
        InventoryRoleType role,
        CancellationToken cancellationToken = default);

    Task<bool> UserHasRoleAsync(
        Guid userId,
        Guid inventoryId,
        InventoryRoleType role,
        CancellationToken cancellationToken = default);
        
    Task<InventoryUserRole?> GetUserRoleInventoryAsync(
        Guid userId,
        Guid inventoryId,
        CancellationToken cancellationToken = default);
}
