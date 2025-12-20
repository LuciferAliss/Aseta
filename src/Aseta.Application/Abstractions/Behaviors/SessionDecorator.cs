using System;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Abstractions.Behaviors;

internal static class SessionDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IUserContext currentUserService,
        IUserSessionChecker userSessionChecker)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Result sessionCheckResult = await CheckSessionAsync(
                currentUserService,
                userSessionChecker,
                cancellationToken);

            return sessionCheckResult.IsFailure
                ? sessionCheckResult.Error
                : await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IUserContext currentUserService,
        IUserSessionChecker userSessionChecker)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Result sessionCheckResult = await CheckSessionAsync(
                currentUserService,
                userSessionChecker,
                cancellationToken);

            return sessionCheckResult.IsFailure
                ? sessionCheckResult.Error
                : await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        IUserContext currentUserService,
        IUserSessionChecker userSessionChecker)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            Result sessionCheckResult = await CheckSessionAsync(
                currentUserService,
                userSessionChecker,
                cancellationToken);

            return sessionCheckResult.IsFailure
                ? sessionCheckResult.Error
                : await innerHandler.Handle(query, cancellationToken);
        }
    }

    private static async Task<Result> CheckSessionAsync(
        IUserContext currentUserService,
        IUserSessionChecker userSessionChecker,
        CancellationToken cancellationToken)
    {
        if (currentUserService.SessionId is null || !currentUserService.IsAuthenticated)
        {
            return Result.Success();
        }

        if (!Guid.TryParse(currentUserService.SessionId, out Guid id))
        {
            return UserSessionErrors.InvalidId(currentUserService.SessionId);
        }

        Result userSessionCheckResult = await userSessionChecker.CheckAsync(id, cancellationToken);
        if (userSessionCheckResult.IsFailure)
        {
            return userSessionCheckResult.Error;
        }

        return Result.Success();
    }
}
