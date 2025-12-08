using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Users.Register;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Users;

internal sealed class Register : IEndpoint
{
    public sealed record Request(
        string UserName,
        string Email,
        string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/register", async (
            Request request,
            ICommandHandler<RegisterUserCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.UserName,
                request.Password);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
