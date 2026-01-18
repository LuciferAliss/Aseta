using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Users.Lock;
using Aseta.Domain.Abstractions.Primitives.Results;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Endpoints.Users;

internal sealed class Lock : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{userId}/lock", async (
            [FromRoute] Guid userId,
            ICommandHandler<LockUserCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            var command = new LockUserCommand(userId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.Users)
        .RequireAuthorization();
    }
}
