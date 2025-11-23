using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Services;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.UserRoles;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Authorization;

internal sealed class UserRoleChecker(
    UserManager<ApplicationUser> userManager,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    ICacheService cacheService) : IUserRoleChecker
{
    private async Task<bool> HasAdminRoleAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;

        return await userManager.IsInRoleAsync(user, "Admin");
    }

    public async Task<bool> HasPermissionAsync(string userId, Guid inventoryId, Role requiredRole, CancellationToken cancellationToken = default)
    {
        var hasPermission = await cacheService.GetAsync($"has-permission-{userId}-{inventoryId}",
            async () =>
            {
                var role = await inventoryUserRoleRepository.GetUserRoleInInventory(userId, inventoryId, cancellationToken);
                if (role >= requiredRole) return true;
                
                return await HasAdminRoleAsync(userId, cancellationToken);
            },
            cancellationToken);

        return hasPermission;
    }
}