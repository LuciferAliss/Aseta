using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.Update;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.API.Endpoints.Inventories;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string Name,
        string Description,
        string ImageUrl,
        bool IsPublic,
        string CategoryId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/inventories/update/{InventoryId}", async (
            string InventoryId,
            Request request,
            ICommandHandler<UpdateInventoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            _ = Guid.TryParse(InventoryId, out Guid inventoryId);
            _ = Guid.TryParse(request.CategoryId, out Guid categoryId);
            _ = Uri.TryCreate(request.ImageUrl, UriKind.Absolute, out Uri? imageUrl);

            var command = new UpdateInventoryCommand(
                inventoryId,
                request.Name,
                request.Description,
                imageUrl,
                categoryId,
                request.IsPublic);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Inventories)
        .RequireAuthorization();
    }
}
