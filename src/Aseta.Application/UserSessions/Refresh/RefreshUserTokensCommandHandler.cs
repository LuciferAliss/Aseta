using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;
using Microsoft.Extensions.Options;

namespace Aseta.Application.UserSessions.Refresh;

internal sealed class RefreshUserTokensCommandHandler(
    IUserSessionRepository userSessionRepository,
    ITokenProvider tokenProvider,
    IUnitOfWork unitOfWork) : ICommandHandler<RefreshUserTokensCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(
        RefreshUserTokensCommand command,
        CancellationToken cancellationToken)
    {
        UserSession? oldSession = await userSessionRepository.GetByRefreshTokenAsync(command.RefreshToken);

        if (oldSession is null)
        {
            return UserSessionErrors.NotFound();
        }

        if (oldSession.IsRevoked)
        {
            await userSessionRepository.RevokeAllForUserAsync(oldSession.UserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return UserSessionErrors.SuspiciousActivity();
        }

        if (oldSession.IsExpired)
        {
            return UserSessionErrors.TokenIsInactive();
        }

        oldSession.Revoke();

        (string newRefreshToken, int ExpiresIn) = tokenProvider.CreateRefreshToken();
        Result<UserSession> newSessionResult = UserSession.Create(
            newRefreshToken,
            oldSession.UserId,
            ExpiresIn,
            oldSession.DeviceId,
            oldSession.DeviceName);

        if (newSessionResult.IsFailure)
        {
            return newSessionResult.Error;
        }

        UserSession userSession = newSessionResult.Value;

        await userSessionRepository.AddAsync(userSession, cancellationToken);

        string accessToken = tokenProvider.CreateAccessToken(oldSession.User, userSession.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new TokenResponse(accessToken, userSession);
    }
}