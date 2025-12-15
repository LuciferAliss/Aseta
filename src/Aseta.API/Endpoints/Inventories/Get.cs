using System;
using Aseta.Application.Inventories.Get;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.Get.Contracts;
using Aseta.API.Infrastructure;
using Aseta.API.Extensions;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Inventories;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/inventories/{InventoryId}", async (
            string InventoryId,
            IQueryHandler<GetInventoryQuery, InventoryResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(InventoryId, out Guid inventoryId);

            var query = new GetInventoryQuery(inventoryId);

            Result<InventoryResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Inventories);
    }
}
