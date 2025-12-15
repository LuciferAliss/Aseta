using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.GetPaginated;
using Aseta.Application.Inventories.GetPaginated.Contracts;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.DTO.Inventories;
using Microsoft.AspNetCore.Http;

namespace Aseta.API.Endpoints.Inventories;

internal sealed class GetPaginated : IEndpoint
{
    public sealed record Request(
        DateTime? CreatedAtFrom,
        DateTime? CreatedAtTo,
        string[]? TagIds,
        string[]? CategoryIds,
        int? MinItemsCount,
        int? MaxItemsCount,
        string? Cursor,
        int PageSize = 0,
        string SortBy = "Date",
        string SortOrder = "desc");

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/inventories", async (
            [AsParameters] Request request,
            IQueryHandler<GetInventoriesPaginatedQuery, InventoriesResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Enum.TryParse(request.SortBy, out SortBy sortBy);
            ICollection<Guid> tagIds = request.TagIds?.Select(t =>
            {
                _ = Guid.TryParse(t, out Guid tagId);
                return tagId;
            }).ToList() ?? [];
            ICollection<Guid> categoryIds = request.CategoryIds?.Select(c =>
            {
                _ = Guid.TryParse(c, out Guid tagId);
                return tagId;
            }).ToList() ?? [];

            var query = new GetInventoriesPaginatedQuery(
                request.CreatedAtFrom,
                request.CreatedAtTo,
                tagIds,
                categoryIds,
                request.MinItemsCount,
                request.MaxItemsCount,
                sortBy,
                request.SortOrder,
                request.Cursor,
                request.PageSize
            );

            Result<InventoriesResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Inventories);
    }
}
