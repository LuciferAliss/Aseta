using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.UserSessions.Logout;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.API.Endpoints.UserSessions;

internal sealed class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/user-sessions", async (
            ICommandHandler<LogoutUserCommand> handler,
            HttpContext httpContext,
            CancellationToken cancellationToken = default
            ) =>
        {
            string? refreshToken = httpContext.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return CustomResults.Problem(UserSessionErrors.TokenEmpty());
            }

            var command = new LogoutUserCommand(refreshToken);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(
                () => {
                    httpContext.Response.Cookies.Delete("refreshToken");
                    return Results.NoContent();
                },
                CustomResults.Problem
            );
        })
        .WithTags(Tags.UserSessions)
        .RequireAuthorization();
    }
}
