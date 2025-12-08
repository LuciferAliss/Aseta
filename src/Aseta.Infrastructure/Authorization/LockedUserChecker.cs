using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Services;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Authorization;

internal sealed class LockedUserChecker(
    IUserRepository userRepository,
    ICacheService cacheService) : ILockedUserChecker
{
    public async Task<bool> IsLockedAsync(string userId, CancellationToken cancellationToken = default)
    {
        bool isLocked = await cacheService.GetAsync(
            $"locked-{userId}",
            async () =>
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

                return user.IsLocked;
            },
            cancellationToken);

        await cacheService.SetAsync($"locked-{userId}", isLocked, cancellationToken);

        return isLocked;
    }
}
