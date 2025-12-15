using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Items.GetPaginated.Contracts;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Pagination;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.DTO.Items;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;
using AutoMapper;

namespace Aseta.Application.Items.GetPaginated;

internal sealed class GetItemsPaginatedQueryHandler(
    IItemRepository itemRepository) : IQueryHandler<GetItemsPaginatedQuery, ItemsResponse>
{
    public async Task<Result<ItemsResponse>> Handle(
        GetItemsPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        var paginationParameters = new ItemPaginationParameters(
            query.CreatedAtFrom,
            query.CreatedAtTo,
            query.UpdatedAtFrom,
            query.UpdatedAtTo,
            query.CreatorId,
            query.UpdaterId,
            query.SortBy,
            query.SortOrder,
            query.Cursor,
            query.PageSize);

        (ICollection<Item>? items, string? nextCursor, bool hasNextPage) = await itemRepository
            .GetPaginatedWithKeysetAsync(paginationParameters, query.InventoryId, cancellationToken);

        ICollection<ItemResponse> itemResponses = items.Select(i => new ItemResponse(
            i.Id,
            i.CustomId,
            i.CustomFieldValues.Select(cfv => new CustomFieldValueResponse(
                cfv.FieldId,
                cfv.Value)).ToList(),
            i.Creator.UserName,
            i.Updater?.UserName,
            i.CreatedAt,
            i.UpdatedAt)).ToList();

        var paginationResult = new KeysetPage<ItemResponse>(nextCursor, hasNextPage, itemResponses);

        return new ItemsResponse(paginationResult);
    }
}