using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.GetPaginated.Contracts;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Pagination;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.DTO.Inventories;
using Aseta.Domain.Entities.Inventories;
using AutoMapper;

namespace Aseta.Application.Inventories.GetPaginated;

internal sealed class GetPaginatedInventoryQueryHandler(
    IMapper mapper,
    IInventoryRepository inventoryRepository) : IQueryHandler<GetPaginatedInventoryQuery, InventoriesResponse>
{
    public async Task<Result<InventoriesResponse>> Handle(
        GetPaginatedInventoryQuery query,
        CancellationToken cancellationToken)
    {
        InventoryPaginationParameters paginationParameters = mapper.Map<InventoryPaginationParameters>(query);

        (ICollection<Inventory>? inventories, string? nextCursor, bool hasNextPage) = await inventoryRepository.GetPaginatedWithKeysetAsync(paginationParameters, cancellationToken);

        ICollection<InventoryResponse> inventoryResponses = mapper.Map<ICollection<InventoryResponse>>(inventories);

        var paginationResult = new KeysetPage<InventoryResponse>(nextCursor, hasNextPage, inventoryResponses);

        return new InventoriesResponse(paginationResult);
    }
}
