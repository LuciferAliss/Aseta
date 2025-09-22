using Aseta.Domain.Entities.Inventories;
using Microsoft.AspNetCore.Authorization;

namespace Aseta.Infrastructure.Requirements;

public class InventoryRoleRequirement : IAuthorizationRequirement
{
    public InventoryRoleType RequiredRole { get; }

    public InventoryRoleRequirement(InventoryRoleType requiredRole)
    {
        RequiredRole = requiredRole;
    }
}