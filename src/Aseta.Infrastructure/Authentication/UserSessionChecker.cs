using System;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Infrastructure.Authentication;

public class UserSessionChecker(IUserSessionRepository userSessionRepository) : IUserSessionChecker
{
    public async Task<Result> CheckAsync(Guid id, CancellationToken cancellationToken = default)
    {
        UserSession? userSession = await userSessionRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (userSession is null)
        {
            return UserSessionErrors.NotFound();
        }

        if (!userSession.IsActive)
        {
            return UserSessionErrors.TokenIsInactive();
        }

        return Result.Success();
    }
}