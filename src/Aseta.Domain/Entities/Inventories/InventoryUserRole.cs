using Aseta.Domain.Entities.Users;
using Aseta.Domain.Enums;

namespace Aseta.Domain.Entities.Inventories;

public class InventoryUserRole
{
    public Guid UserId { get; private set; }
    public virtual UserApplication User { get; private set; } = null!;

    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; private set; } = null!;

    public Role Role { get; private set; }

    private InventoryUserRole() { }

    private InventoryUserRole(Guid userId, Guid inventoryId, Role role)
    {
        UserId = userId;
        InventoryId = inventoryId;
        Role = role;
    }

    public static InventoryUserRole Create(Guid userId, Guid inventoryId, Role role) => new(userId, inventoryId, role);
}