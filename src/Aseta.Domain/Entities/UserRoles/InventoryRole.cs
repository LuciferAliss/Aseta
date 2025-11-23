using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.UserRoles;

public class InventoryRole : IEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public virtual ApplicationUser User { get; private set; } = null!;
    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; private set; } = null!;
    public Role Role { get; private set; }

    private InventoryRole() { }

    private InventoryRole(Guid userId, Guid inventoryId, Role role)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        InventoryId = inventoryId;
        Role = role;
    }

    public static Result<InventoryRole> Create(Guid userId, Guid inventoryId, Role role) => new InventoryRole(userId, inventoryId, role);
}