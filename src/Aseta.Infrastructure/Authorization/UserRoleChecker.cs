using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Services;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Domain.Entities.Users;

namespace Aseta.Infrastructure.Authorization;

internal sealed class UserRoleChecker(
    IInventoryUserRoleRepository inventoryUserRoleRepository) : IUserRoleChecker
{
    public async Task<bool> HasPermissionAsync(
        Guid userId,
        Guid inventoryId,
        Role requiredRole,
        CancellationToken cancellationToken = default)
    {
        Role role = await inventoryUserRoleRepository.GetUserRoleInInventory(userId, inventoryId, cancellationToken);

        return role >= requiredRole;
    }
}