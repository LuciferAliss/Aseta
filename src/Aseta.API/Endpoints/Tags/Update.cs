using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Tags.Update;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Tags;

internal sealed class Update : IEndpoint
{
    public sealed record Request(string Name);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/tags/{id}", async (
            string id,
            Request request,
            ICommandHandler<UpdateTagCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid tagId);

            var command = new UpdateTagCommand(tagId, request.Name);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.Tags)
        .RequireAuthorization();
    }
}
