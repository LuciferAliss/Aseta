using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.UserRoles;

public class InventoryRole : Entity
{
    public Guid UserId { get; private set; }
    public virtual ApplicationUser User { get; private set; } = null!;
    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; private set; } = null!;
    public Role Role { get; private set; }

    private InventoryRole() { }

    private InventoryRole(Guid id, Guid userId, Guid inventoryId, Role role) : base(id)
    {
        UserId = userId;
        InventoryId = inventoryId;
        Role = role;
    }

    public static Result<InventoryRole> Create(Guid userId, Guid inventoryId, Role role)
    {
        if (role == Role.None)
        {
            return InventoryRoleErrors.InvalidRole();
        }

        return new InventoryRole(Guid.NewGuid(), userId, inventoryId, role);
    }
}