using System.Reflection;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Abstractions.Behaviors;

internal static class AuthorizationDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IUserContext currentUserService,
        IUserRoleChecker userRoleChecker)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(
            TCommand command,
            CancellationToken cancellationToken)
        {
            Result authorizationResult = await AuthorizeRequestAsync(
                command,
                currentUserService,
                userRoleChecker,
                cancellationToken);

            return authorizationResult.IsFailure
                ? authorizationResult.Error
                : await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IUserContext currentUserService,
        IUserRoleChecker userRoleChecker)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Result authorizationResult = await AuthorizeRequestAsync(
                command,
                currentUserService,
                userRoleChecker,
                cancellationToken);

            return authorizationResult.IsFailure
                ? authorizationResult.Error
                : await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        IUserContext currentUserService,
        IUserRoleChecker userRoleChecker)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            Result authorizationResult = await AuthorizeRequestAsync(
                query,
                currentUserService,
                userRoleChecker,
                cancellationToken);

            return authorizationResult.IsFailure
                ? authorizationResult.Error
                : await innerHandler.Handle(query, cancellationToken);
        }
    }

    private static async Task<Result> AuthorizeRequestAsync<TRequest>(
        TRequest request,
        IUserContext currentUserService,
        IUserRoleChecker userRoleChecker,
        CancellationToken cancellationToken)
    {
        AuthorizeAttribute? authorizeAttribute = typeof(TRequest).GetCustomAttribute<AuthorizeAttribute>();

        if (authorizeAttribute is null)
        {
            return Result.Success();
        }

        if (currentUserService.UserId is null || !currentUserService.IsAuthenticated)
        {
            return UserErrors.NotAuthenticated();
        }

        if (authorizeAttribute.UserRole.HasValue &&
            currentUserService.UserRole != authorizeAttribute.UserRole.Value &&
            currentUserService.UserRole != UserRole.Admin)
        {
            return UserErrors.NotGlobalPermission(authorizeAttribute.UserRole.Value.ToString());
        }

        if (authorizeAttribute.InventoryRole != Role.None)
        {
            if (currentUserService.UserRole == UserRole.Admin)
            {
                return Result.Success();
            }

            if (request is IInventoryScopedRequest inventoryScopedRequest)
            {
                if (!Guid.TryParse(currentUserService.UserId, out Guid userId))
                {
                    return UserErrors.InvalidId(currentUserService.UserId);
                }

                bool hasPermission = await userRoleChecker.HasPermissionAsync(
                    userId,
                    inventoryScopedRequest.InventoryId,
                    authorizeAttribute.InventoryRole,
                    cancellationToken);

                if (!hasPermission)
                {
                    return UserErrors.NotPermission(inventoryScopedRequest.InventoryId);
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"The request '{typeof(TRequest).Name}' has an [Authorize(InventoryRole)] attribute but does not implement the {nameof(IInventoryScopedRequest)} interface. Authorization cannot be performed.");
            }
        }

        return Result.Success();
    }
}