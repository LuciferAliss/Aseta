using System.Diagnostics.CodeAnalysis;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.UserRoles;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Domain.Entities.Users;

public class ApplicationUser : IdentityUser<Guid>, IEntity
{
    public new required string UserName { get; set; }
    public virtual ICollection<Inventory> Inventories { get; private set; } = [];
    public virtual ICollection<InventoryRole> InventoryUserRoles { get; private set; } = [];
}