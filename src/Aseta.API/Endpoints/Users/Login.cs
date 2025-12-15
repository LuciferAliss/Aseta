using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Users.Login;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record Request(
        string Email,
        string Password,
        string DeviceId,
        string DeviceName);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/login", async (
            Request request,
            ICommandHandler<LoginUserCommand, LoginResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            var command = new LoginUserCommand(
                request.Email,
                request.Password,
                request.DeviceId,
                request.DeviceName);

            Result<LoginResponse> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
