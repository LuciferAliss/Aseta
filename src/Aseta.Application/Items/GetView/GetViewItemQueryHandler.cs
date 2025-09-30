using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Inventories;
using AutoMapper;

namespace Aseta.Application.Items.GetView;

internal sealed class GetViewItemQueryHandler(
    IInventoryRepository inventoryRepository,
    IItemRepository itemRepository,
    IMapper mapper
) : IQueryHandler<GetViewItemQuery, PaginatedResult<ItemViewResponse>>
{
    public async Task<Result<PaginatedResult<ItemViewResponse>>> Handle(GetViewItemQuery query, CancellationToken cancellationToken)
    {
        bool inventoryExists = await inventoryRepository
            .ExistsAsync(query.InventoryId, cancellationToken);
        if (!inventoryExists) return InventoryErrors.NotFound(query.InventoryId);

        var items = await itemRepository.GetItemsPageAsync(
            query.InventoryId,
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        var itemsResponse = items.Select(mapper.Map<ItemViewResponse>).ToList();

        int totalCount = await itemRepository.CountItems(query.InventoryId, cancellationToken);

        return new PaginatedResult<ItemViewResponse>(
            itemsResponse,
            query.PageNumber,
            query.PageSize,
            totalCount,
            query.PageNumber * query.PageSize < totalCount
        );
    }
}
