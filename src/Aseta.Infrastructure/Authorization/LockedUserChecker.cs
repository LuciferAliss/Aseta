using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Services;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Authorization;

internal sealed class LockedUserChecker(
    UserManager<ApplicationUser> userManager,
    ICacheService cacheService) : ILockedUserChecker
{
    public async Task<bool> IsLockedAsync(string userId, CancellationToken cancellationToken = default)
    {
        bool isLocked = await cacheService.GetAsync(
            $"locked-{userId}",
            async () =>
            {
                ApplicationUser? user = await userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return false;
                }

                return await userManager.IsLockedOutAsync(user);
            },
            cancellationToken);

        await cacheService.SetAsync($"locked-{userId}", isLocked, cancellationToken);

        return isLocked;
    }
}
