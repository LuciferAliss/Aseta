using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Items.Delete;
using Aseta.Domain.Abstractions.Primitives.Results;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Endpoints.Items;

internal sealed class Delete : IEndpoint
{
    public sealed record Request(string[] ItemIds);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/inventories/{id}/items", async (
            string id,
            [FromBody] Request request,
            ICommandHandler<DeleteItemsCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);

            ICollection<Guid> itemIds = request.ItemIds.Select(x =>
            {
                _ = Guid.TryParse(x, out Guid id);
                return id;
            }).ToList();

            var command = new DeleteItemsCommand(itemIds, inventoryId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Items)
        .RequireAuthorization();
    }
}
