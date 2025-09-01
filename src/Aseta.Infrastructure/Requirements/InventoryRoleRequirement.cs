using Aseta.Domain.Entities.Inventories;
using Microsoft.AspNetCore.Authorization;

namespace Aseta.Infrastructure.Requirements;

public class InventoryRoleRequirement : IAuthorizationRequirement
{
    public InventoryRole RequiredRole { get; }

    public InventoryRoleRequirement(InventoryRole requiredRole)
    {
        RequiredRole = requiredRole;
    }
}