using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Inventories;

public class InventoryUserRole
{
    public Guid UserId { get; private set; }
    public virtual UserApplication User { get; private set; }

    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; private set; }

    public InventoryRole Role { get; private set; }

    private InventoryUserRole() { }

    private InventoryUserRole(Guid userId, Guid inventoryId, InventoryRole role)
    {
        UserId = userId;
        InventoryId = inventoryId;
        Role = role;
    }

    public static InventoryUserRole Create(Guid userId, Guid inventoryId, InventoryRole role) => new(userId, inventoryId, role);
}