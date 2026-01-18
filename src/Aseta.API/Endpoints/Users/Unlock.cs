using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Users.Unlock;
using Aseta.Domain.Abstractions.Primitives.Results;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Endpoints.Users;

internal sealed class Unlock : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{userId}/unlock", async (
            [FromRoute] Guid userId,
            ICommandHandler<UnlockUserCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            var command = new UnlockUserCommand(userId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.Users)
        .RequireAuthorization();
    }
}
