using Aseta.Domain.Entities.Inventories;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Domain.Entities.Users;

public class UserApplication : IdentityUser<Guid>
{
    public virtual List<Inventory> Inventories { get; private set; } = [];
    public virtual List<InventoryUserRole> InventoryUserRoles { get; private set; } = [];

    public Guid RoleId { get; private set; }
    public virtual IdentityRole<Guid> Role { get; private set; }
}