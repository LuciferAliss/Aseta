using Aseta.Domain.Entities.Users;
using Aseta.Domain.Enums;

namespace Aseta.Domain.Entities.Inventories;

public class InventoryRole
{
    public Guid UserId { get; private set; }
    public virtual UserApplication User { get; private set; } = null!;

    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; private set; } = null!;

    public Role Role { get; private set; }

    private InventoryRole() { }

    private InventoryRole(Guid userId, Guid inventoryId, Role role)
    {
        UserId = userId;
        InventoryId = inventoryId;
        Role = role;
    }

    public static InventoryRole Create(Guid userId, Guid inventoryId, Role role) => new(userId, inventoryId, role);
}