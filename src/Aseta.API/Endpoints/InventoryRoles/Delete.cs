using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.InventoryRoles.Delete;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.InventoryRoles;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/inventories/{id}/roles/{userId}", async (
            string id,
            string userId,
            ICommandHandler<DeleteRoleUserCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);
            _ = Guid.TryParse(userId, out Guid deletedUserId);

            var command = new DeleteRoleUserCommand(inventoryId, deletedUserId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.InventoryRoles)
        .RequireAuthorization();
    }
}
