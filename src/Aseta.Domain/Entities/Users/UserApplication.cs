using Aseta.Domain.Entities.Inventories;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Domain.Entities.Users;

public class UserApplication : IdentityUser<Guid>
{
    public virtual ICollection<Inventory> Inventories { get; private set; } = [];
    public virtual ICollection<InventoryUserRole> InventoryUserRoles { get; private set; } = [];
}