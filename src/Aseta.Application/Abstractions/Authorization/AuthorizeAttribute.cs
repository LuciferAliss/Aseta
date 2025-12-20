using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Abstractions.Authorization;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class AuthorizeAttribute : Attribute
{
    public Role InventoryRole { get; }
    public UserRole? UserRole { get; }

    public AuthorizeAttribute()
    {
        InventoryRole = Role.None;
        UserRole = null;
    }

    public AuthorizeAttribute(Role inventoryRole)
    {
        InventoryRole = inventoryRole;
        UserRole = null;
    }

    public AuthorizeAttribute(UserRole userRole)
    {
        InventoryRole = Role.None;
        UserRole = userRole;
    }
}