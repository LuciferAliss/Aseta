using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.InventoryRoles.GetUser;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.InventoryRoles;

internal sealed class GetUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/inventories/{id}/roles/users", async (
            string id,
            IQueryHandler<GetInventoryUserQuery, UsersResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);

            var query = new GetInventoryUserQuery(inventoryId);

            Result<UsersResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(TagsApi.InventoryRoles)
        .RequireAuthorization();
    }
}
