using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Services;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Domain.Entities.Users;

namespace Aseta.Infrastructure.Authorization;

internal sealed class UserRoleChecker(
    IUserRepository userRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    ICacheService cacheService) : IUserRoleChecker
{
    private async Task<bool> HasAdminRoleAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(userId, out Guid id))
        {
            return false;
        }

        User? user = await userRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (user is null)
        {
            return false;
        }

        return user.Role == UserRole.Admin;
    }

    public async Task<bool> HasPermissionAsync(string userId, Guid inventoryId, Role requiredRole, CancellationToken cancellationToken = default)
    {
        bool hasPermission = await cacheService.GetAsync($"has-permission-{userId}-{inventoryId}",
            async () =>
            {
                Role role = await inventoryUserRoleRepository.GetUserRoleInInventory(userId, inventoryId, cancellationToken);
                if (role >= requiredRole)
                {
                    return true;
                }

                return await HasAdminRoleAsync(userId, cancellationToken);
            },
            cancellationToken);

        return hasPermission;
    }
}