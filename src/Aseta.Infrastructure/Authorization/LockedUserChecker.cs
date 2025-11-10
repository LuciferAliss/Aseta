using Aseta.Application.Abstractions.Authorization;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Authorization;

internal sealed class LockedUserChecker(
    UserManager<UserApplication> userManager) : ILockedUserChecker
{
    public async Task<bool> IsLockedAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;

        return await userManager.IsLockedOutAsync(user);
    }
}
