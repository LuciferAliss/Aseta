using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.Get.Contracts;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories;
using AutoMapper;

namespace Aseta.Application.Inventories.Get;

internal sealed class GetInventoryQueryHandler(
    IInventoryRepository inventoryRepository,
    IMapper mapper) : IQueryHandler<GetInventoryQuery, InventoryResponse>
{
    public async Task<Result<InventoryResponse>> Handle(
        GetInventoryQuery query,
        CancellationToken cancellationToken)
    {
        var inventory = await inventoryRepository.GetByIdAsync(
            query.InventoryId,
            false,
            cancellationToken,
            i => i.Tags,
            i => i.UserRoles,
            i => i.Category,
            i => i.Creator);
            
        if (inventory is null) return InventoryErrors.NotFound(query.InventoryId);
        
        var response = mapper.Map<InventoryResponse>(inventory);

        return response;
    }
}