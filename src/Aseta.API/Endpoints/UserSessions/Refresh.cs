using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.UserSessions.Refresh;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.API.Endpoints.UserSessions;

internal sealed class Refresh : IEndpoint
{
    public sealed record Response(string AccessToken);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/user-sessions/refresh", async (
            HttpContext httpContext,
            ICommandHandler<RefreshUserTokensCommand, TokenResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            string? refreshToken = httpContext.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return CustomResults.Problem(UserSessionErrors.TokenEmpty());
            }

            var command = new RefreshUserTokensCommand(refreshToken);

            Result<TokenResponse> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                response =>
                {
                    httpContext.Response.Cookies.Append(
                        "refreshToken",
                        response.UserSession.Token,
                        new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = response.UserSession.ExpiresAt,
                            SameSite = SameSiteMode.Strict,
                            Secure = true,
                        });

                    return Results.Ok(new Response(response.AccessToken));
                },
                CustomResults.Problem);
        }
        ).WithTags(TagsApi.UserSessions);
    }
}
