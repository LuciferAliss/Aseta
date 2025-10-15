using Aseta.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Aseta.Infrastructure.InventoryRole;

public class InventoryRoleRequirement(Role requiredRole) : IAuthorizationRequirement
{
    public Role RequiredRole { get; } = requiredRole;
}