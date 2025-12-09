using System;

namespace Aseta.API.Endpoints.Inventories;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/inventory/{InventoryId}", async (
            string InventoryId,
            ICommandHandler<DeleteInventoryCommand> handler,
            CancellationToken cancellationToken) =>
        {

        });
    }
}
