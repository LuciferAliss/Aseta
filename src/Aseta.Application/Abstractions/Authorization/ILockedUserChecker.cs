namespace Aseta.Application.Abstractions.Authorization;

public interface ILockedUserChecker
{
    Task<bool> IsLockedAsync(string userId, CancellationToken cancellationToken = default);
}