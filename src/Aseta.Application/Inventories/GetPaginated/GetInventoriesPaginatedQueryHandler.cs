using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.GetPaginated.Contracts;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Pagination;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.DTO.Inventories;
using Aseta.Domain.Entities.Inventories;
using AutoMapper;

namespace Aseta.Application.Inventories.GetPaginated;

internal sealed class GetInventoriesPaginatedQueryHandler(
    IInventoryRepository inventoryRepository) : IQueryHandler<GetInventoriesPaginatedQuery, InventoriesResponse>
{
    public async Task<Result<InventoriesResponse>> Handle(
        GetInventoriesPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        var paginationParameters = new InventoryPaginationParameters(
            query.CreatedAtFrom,
            query.CreatedAtTo,
            query.TagIds,
            query.CategoryIds,
            query.MinItemsCount,
            query.MaxItemsCount,
            query.SortBy,
            query.SortOrder,
            query.Cursor,
            query.PageSize
        );

        (ICollection<Inventory>? inventories, string? nextCursor, bool hasNextPage) = await inventoryRepository
            .GetPaginatedWithKeysetAsync(paginationParameters, cancellationToken);

        ICollection<InventoryResponse> inventoryResponses = inventories.Select(i =>
        {
            if (!Uri.TryCreate(i.ImageUrl, UriKind.Absolute, out Uri? imageUrl))
            {
                return null;
            }

            var response = new InventoryResponse(
                i.Id,
                i.Name,
                i.Description,
                imageUrl,
                i.ItemsCount,
                i.Creator.UserName,
                i.CreatedAt);

            return response;
        }).Where(i => i is not null).Select(i => i!).ToList();

        var paginationResult = new KeysetPage<InventoryResponse>(nextCursor, hasNextPage, inventoryResponses);

        return new InventoriesResponse(paginationResult);
    }
}
