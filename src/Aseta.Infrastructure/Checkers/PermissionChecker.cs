using Aseta.Application.Abstractions.Authorization;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Users;
using Aseta.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Checkers;

public class PermissionChecker(
    UserManager<UserApplication> userManager,
    IInventoryUserRoleRepository inventoryUserRoleRepository) : IPermissionChecker
{
    public async Task<Result<bool>> HasPermissionAsync(
        string userId,
        Guid inventoryId,
        Role requiredRole,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return UserErrors.NotFound(userId);
        }

        var hasPermission = await userManager.IsInRoleAsync(user, Role.Admin.ToString()) ||
            await inventoryUserRoleRepository.HasUserRoleAsync(
                userId,
                inventoryId,
                requiredRole,
                cancellationToken);
        
        return hasPermission;
    }
}
