using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Inventories;

public class InventoryUserRole(Guid userId, Guid inventoryId, InventoryRoleType role)
{
    public Guid UserId { get; private set; } = userId;
    public virtual UserApplication User { get; private set; } = null!;

    public Guid InventoryId { get; private set; } = inventoryId;
    public virtual Inventory Inventory { get; private set; } = null!;

    public InventoryRoleType Role { get; private set; } = role;
}