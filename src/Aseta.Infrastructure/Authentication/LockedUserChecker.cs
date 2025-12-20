using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Services;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Authentication;

internal sealed class LockedUserChecker(
    IUserRepository userRepository) : ILockedUserChecker
{
    public async Task<bool> CheckAsync(string userId, CancellationToken cancellationToken = default)
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
    }
}
