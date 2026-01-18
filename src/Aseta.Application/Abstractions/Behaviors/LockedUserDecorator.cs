using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Abstractions.Behaviors;

internal static class LockedUserDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IUserContext currentUserService,
        ILockedUserChecker lockedUserChecker)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Result lockedUserResult = await CheckingUserBlocking(
                currentUserService,
                lockedUserChecker,
                cancellationToken);

            return lockedUserResult.IsFailure
                ? lockedUserResult.Error
                : await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IUserContext currentUserService,
        ILockedUserChecker lockedUserChecker)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Result lockedUserResult = await CheckingUserBlocking(
                currentUserService,
                lockedUserChecker,
                cancellationToken);

            return lockedUserResult.IsFailure
                ? lockedUserResult.Error
                : await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        IUserContext currentUserService,
        ILockedUserChecker lockedUserChecker)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            Result lockedUserResult = await CheckingUserBlocking(
                currentUserService,
                lockedUserChecker,
                cancellationToken);

            return lockedUserResult.IsFailure
                ? lockedUserResult.Error
                : await innerHandler.Handle(query, cancellationToken);
        }
    }

    private static async Task<Result> CheckingUserBlocking(
        IUserContext currentUserService,
        ILockedUserChecker lockedUserChecker,
        CancellationToken cancellationToken)
    {
        if (currentUserService.UserId is null || !currentUserService.IsAuthenticated)
        {
            return Result.Success();
        }

        bool userLocked = await lockedUserChecker.CheckAsync(currentUserService.UserId, cancellationToken);
        if (userLocked)
        {
            return UserErrors.AccountLocked(currentUserService.UserId);
        }

        return Result.Success();
    }
}

