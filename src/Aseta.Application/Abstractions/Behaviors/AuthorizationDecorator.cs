using System.Reflection;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Checkers;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Users;
using Aseta.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Aseta.Application.Abstractions.Behaviors;

internal static class AuthorizationDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IHttpContextAccessor httpContextAccessor,
        ICheckingAccessPolicy accessPolicyChecker)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var authorizeAttribute = typeof(TCommand).GetCustomAttribute<AuthorizeAttribute>();

            if (authorizeAttribute is null)
            {
                return await innerHandler.Handle(command, cancellationToken);
            }

            var user = httpContextAccessor.HttpContext?.User;

            if (user is null || !user.Identity?.IsAuthenticated == true)
            {
                return Result.Failure<TResponse>(UserErrors.NotFound());
            }
            
            if (authorizeAttribute.Role != Role.None)
            {
                var userRoles = Enum.GetValues<Role>()
                                    .Where(r => user.IsInRole(r.ToString()));

                if (!userRoles.Contains(authorizeAttribute.Role))
                {
                    return Result.Failure<TResponse>(UserErrors.NotPermission(Guid.Empty));
                }
            }
            
            if (command is IResourceBasedRequest resourceRequest)
            {
                bool hasAccess = await accessPolicyChecker.CheckAsync(
                    user,
                    resourceRequest.ResourceId,
                    authorizeAttribute.Permission);

                if (!hasAccess)
                {
                    return Result.Failure<TResponse>(UserErrors.NotPermission(Guid.Empty));
                }
            }

            return await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}