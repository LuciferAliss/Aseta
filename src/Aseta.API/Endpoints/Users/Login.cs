using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Users.Login;
using Aseta.Domain.Abstractions.Primitives.Results;
using Microsoft.AspNetCore.Http;

namespace Aseta.API.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record Request(
        string Email,
        string Password,
        string DeviceId,
        string DeviceName);

    public sealed record Response(string AccessToken);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/login", async (
            Request request,
            ICommandHandler<LoginUserCommand, LoginResponse> handler,
            HttpContext httpContext,
            CancellationToken cancellationToken = default) =>
        {
            var command = new LoginUserCommand(
                request.Email,
                request.Password,
                request.DeviceId,
                request.DeviceName);

            Result<LoginResponse> result = await handler.Handle(command, cancellationToken);

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
        })
        .WithTags(Tags.Users);
    }
}
