using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.UserSessions.Logout;

internal sealed class LogoutUserCommandHandler(
    IUserSessionRepository userSessionRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<LogoutUserCommand>
{
    public async Task<Result> Handle(LogoutUserCommand command, CancellationToken cancellationToken)
    {
        UserSession? userSessions = await userSessionRepository.GetByRefreshTokenAsync(command.RefreshToken);

        if (userSessions is null)
        {
            return UserSessionErrors.NotFound();
        }

        userSessions.Revoke();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
