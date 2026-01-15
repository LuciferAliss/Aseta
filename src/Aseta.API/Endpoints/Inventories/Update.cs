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
        string CategoryId,
        string[] TagIds);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/inventories/{id}", async (
            string id,
            Request request,
            ICommandHandler<UpdateInventoryCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);
            _ = Guid.TryParse(request.CategoryId, out Guid categoryId);
            _ = Uri.TryCreate(request.ImageUrl, UriKind.Absolute, out Uri? imageUrl);
            ICollection<Guid> tagIds = request.TagIds?.Select(t =>
            {
                _ = Guid.TryParse(t, out Guid tagId);
                return tagId;
            }).ToList() ?? [];

            var command = new UpdateInventoryCommand(
                inventoryId,
                request.Name,
                request.Description,
                imageUrl,
                categoryId,
                tagIds,
                request.IsPublic);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.Inventories)
        .RequireAuthorization();
    }
}
