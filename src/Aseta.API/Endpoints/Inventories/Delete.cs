using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.Delete;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Inventories;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/inventories/delete/{InventoryId}", async (
            string InventoryId,
            ICommandHandler<DeleteInventoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            _ = Guid.TryParse(InventoryId, out Guid inventoryId);

            var command = new DeleteInventoryCommand(inventoryId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Inventories)
        .RequireAuthorization();
    }
}
