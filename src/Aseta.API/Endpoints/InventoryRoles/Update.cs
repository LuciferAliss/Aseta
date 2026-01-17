using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.InventoryRoles.Update;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.API.Endpoints.InventoryRoles;

internal sealed class Update : IEndpoint
{
    public sealed record Request(string Role);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/inventories/{id}/roles/{userId}", async (
            string id,
            string userId,
            Request request,
            ICommandHandler<UpdateRoleUserCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);
            _ = Guid.TryParse(userId, out Guid userIdGuid);
            _ = Enum.TryParse(request.Role, out Role role);

            var command = new UpdateRoleUserCommand(inventoryId, userIdGuid, role);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.InventoryRoles)
        .RequireAuthorization();
    }
}