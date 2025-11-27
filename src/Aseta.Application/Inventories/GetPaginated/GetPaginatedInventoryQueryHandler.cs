using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.DTO.Inventory;
using Aseta.Domain.Entities.Inventories;
using AutoMapper;

namespace Aseta.Application.Inventories.GetPaginated;

internal sealed class GetPaginatedInventoryQueryHandler(
    IMapper mapper,
    IInventoryRepository inventoryRepository) : IQueryHandler<GetPaginatedInventoryQuery, InventoryResponse>
{
    public async Task<Result<InventoryResponse>> Handle(
        GetPaginatedInventoryQuery query,
        CancellationToken cancellationToken)
    {
        var paginationParameters = mapper.Map<InventoryPaginationParameters>(query);

        var (inventories, nextCursor, hasNextPage) = await inventoryRepository.GetPaginatedWithKeysetAsync(paginationParameters, cancellationToken);

        var paginationResult = new KeysetPage<Inventory>(nextCursor, hasNextPage, inventories);

        return new InventoryResponse(paginationResult);
    }
}
