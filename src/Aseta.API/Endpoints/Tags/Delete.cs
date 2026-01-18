using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Tags.Delete;
using Aseta.Domain.Abstractions.Primitives.Results;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Endpoints.Tags;

internal sealed class Delete : IEndpoint
{
    public sealed record Request(string[] TagIds);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/tags", async (
            [FromBody] Request request,
            ICommandHandler<DeleteTagsCommand> handler,
            CancellationToken cancellationToken = default) =>
            {
                ICollection<Guid> tagIds = request.TagIds.Select(x =>
                {
                    _ = Guid.TryParse(x, out Guid id);
                    return id;
                }).ToList();

                var command = new DeleteTagsCommand(tagIds);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            }
        )
        .WithTags(TagsApi.Tags)
        .RequireAuthorization();
    }
}
