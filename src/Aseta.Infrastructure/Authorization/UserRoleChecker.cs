using Aseta.Application.Abstractions.Authorization;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Users;
using Aseta.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Authorization;

internal sealed class UserRoleChecker(
    UserManager<UserApplication> userManager,
    IInventoryUserRoleRepository inventoryUserRoleRepository) : IUserRoleChecker
{
    public async Task<bool> HasAdminRoleAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;

        return await userManager.IsInRoleAsync(user, "Admin");
    }

    public async Task<bool> HasPermissionAsync(string userId, Guid inventoryId, Role requiredRole, CancellationToken cancellationToken = default) =>
        await inventoryUserRoleRepository.HasUserRoleAsync(userId, inventoryId, requiredRole, cancellationToken);
}