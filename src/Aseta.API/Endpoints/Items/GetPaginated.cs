using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Items.GetPaginated;
using Aseta.Application.Items.GetPaginated.Contracts;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.DTO.Items;

namespace Aseta.API.Endpoints.Items;

internal sealed class GetPaginated : IEndpoint
{
    public sealed record Request(
        DateTime? CreatedAtFrom,
        DateTime? CreatedAtTo,
        DateTime? UpdatedAtFrom,
        DateTime? UpdatedAtTo,
        string? CreatorId,
        string? UpdaterId,
        string? Cursor,
        int PageSize = 20,
        string SortBy = "DateCreated",
        string SortOrder = "desc");

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/inventories/{InventoryId}/items", async (
            [AsParameters] Request request,
            string InventoryId,
            IQueryHandler<GetItemsPaginatedQuery, ItemsResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(InventoryId, out Guid inventoryId);
            _ = Enum.TryParse(request.SortBy, out SortBy sortBy);

            Guid? creatorId = null;

            if (request.CreatorId is not null)
            {
                _ = Guid.TryParse(request.CreatorId, out Guid creator);

                creatorId = creator;
            }

            Guid? updaterId = null;

            if (request.UpdaterId is not null)
            {
                _ = Guid.TryParse(request.UpdaterId, out Guid updater);
                updaterId = updater;
            }

            var query = new GetItemsPaginatedQuery(
                inventoryId,
                request.CreatedAtFrom,
                request.CreatedAtTo,
                request.UpdatedAtFrom,
                request.UpdatedAtTo,
                creatorId,
                updaterId,
                sortBy,
                request.SortOrder,
                request.Cursor,
                request.PageSize);

            Result<ItemsResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Items);
    }
}