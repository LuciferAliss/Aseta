using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories;
using AutoMapper;

namespace Aseta.Application.Items.GetView;

internal sealed class GetViewItemQueryHandler(
    IInventoryRepository inventoryRepository,
    IItemRepository itemRepository,
    IMapper mapper) : IQueryHandler<GetViewItemQuery, PaginatedResult<ItemResponse>>
{
    public async Task<Result<PaginatedResult<ItemResponse>>> Handle(
        GetViewItemQuery query,
        CancellationToken cancellationToken)
    {
        bool inventoryExists = await inventoryRepository.ExistsAsync(
                i => i.Id == query.InventoryId,
                cancellationToken);
        if (!inventoryExists) return InventoryErrors.NotFound(query.InventoryId);

        var items = await itemRepository.GetPagedAsync(
            query.PageNumber,
            query.PageSize,
            i => i.InventoryId == query.InventoryId,
            i => i.CreatedAt,
            true,
            cancellationToken);

        var itemsResponse = items.Select(mapper.Map<ItemResponse>).ToList();

        int totalCount = await itemRepository.CountAsync(
            i => i.InventoryId == query.InventoryId,
            cancellationToken);

        return PaginatedResult<ItemResponse>.Create(
            itemsResponse,
            query.PageNumber,
            query.PageSize,
            totalCount);
    }
}