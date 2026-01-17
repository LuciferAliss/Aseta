using System;
using Aseta.Application.Inventories.Get;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.Get.Contracts;
using Aseta.API.Infrastructure;
using Aseta.API.Extensions;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Application.Abstractions.Authentication;

namespace Aseta.API.Endpoints.Inventories;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/inventories/{id}", async (
            string id,
            IQueryHandler<GetInventoryQuery, InventoryResponse> handler,
            IUserContext user,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);
            _ = Guid.TryParse(user.UserId, out Guid userId);

            var query = new GetInventoryQuery(inventoryId, userId);

            Result<InventoryResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(TagsApi.Inventories);
    }
}
