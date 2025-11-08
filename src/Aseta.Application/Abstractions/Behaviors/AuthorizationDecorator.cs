using System.Reflection;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Abstractions.Behaviors;

internal static class AuthorizationDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ICurrentUserService currentUserService,
        IPermissionChecker permissionChecker)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(
            TCommand command,
            CancellationToken cancellationToken)
        {
            var authorizationResult = await AuthorizeRequestAsync(
                command,
                currentUserService,
                permissionChecker,
                cancellationToken);

            if (authorizationResult.IsFailure)
            {
                return Result.Failure<TResponse>(authorizationResult.Error);
            }

            return await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ICurrentUserService currentUserService,
        IPermissionChecker permissionChecker)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var authorizationResult = await AuthorizeRequestAsync(
                command,
                currentUserService,
                permissionChecker,
                cancellationToken);

            if (authorizationResult.IsFailure)
            {
                return authorizationResult;
            }

            return await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ICurrentUserService currentUserService,
        IPermissionChecker permissionChecker)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            var authorizationResult = await AuthorizeRequestAsync(
                query,
                currentUserService,
                permissionChecker,
                cancellationToken);

            if (authorizationResult.IsFailure)
            {
                return Result.Failure<TResponse>(authorizationResult.Error);
            }

            return await innerHandler.Handle(query, cancellationToken);
        }
    }

    private static async Task<Result> AuthorizeRequestAsync<TRequest>(
        TRequest request,
        ICurrentUserService currentUserService,
        IPermissionChecker permissionChecker,
        CancellationToken cancellationToken)
    {
        var authorizeAttribute = typeof(TRequest).GetCustomAttribute<AuthorizeAttribute>();

        if (authorizeAttribute is null)
        {
            return Result.Success();
        }

        if (currentUserService.UserId is null || !currentUserService.IsAuthenticated)
        {
            return Result.Failure(UserErrors.NotAuthenticated());
        }

        if (request is IInventoryScopedRequest inventoryScopedRequest)
        {
            var permissionResult = await permissionChecker.HasPermissionAsync(
                currentUserService.UserId,
                inventoryScopedRequest.InventoryId,
                authorizeAttribute.Role,
                cancellationToken);

            if (permissionResult.IsFailure)
            {
                return Result.Failure(UserErrors.NotPermission(Guid.Empty));
            }
        }

        return Result.Success();
    }
}
