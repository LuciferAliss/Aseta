namespace Aseta.Application.Abstractions.Authentication;

public interface ILockedUserChecker
{
    Task<bool> CheckAsync(string userId, CancellationToken cancellationToken = default);
}