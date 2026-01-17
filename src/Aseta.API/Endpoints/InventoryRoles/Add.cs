using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.InventoryRoles.Add;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.API.Endpoints.InventoryRoles;

internal sealed class Add : IEndpoint
{
    public sealed record Request(string UserId, string Role);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/{id}/roles", async (
            string id,
            Request request,
            ICommandHandler<AddRoleUserCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);
            _ = Guid.TryParse(request.UserId, out Guid userId);
            _ = Enum.TryParse(request.Role, out Role role);

            var command = new AddRoleUserCommand(inventoryId, userId, role);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(TagsApi.InventoryRoles)
        .RequireAuthorization();
    }
}
