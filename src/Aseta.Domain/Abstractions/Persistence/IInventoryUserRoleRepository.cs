using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Enums;

namespace Aseta.Domain.Abstractions.Persistence;

public interface IInventoryUserRoleRepository : IRepository<InventoryRole>
{
    Task<bool> HasUserRoleAsync(string userId, Guid inventoryId, Role role, CancellationToken cancellationToken = default);
}