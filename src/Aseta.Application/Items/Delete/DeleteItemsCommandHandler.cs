using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.Items.Delete;

internal sealed class DeleteItemsCommandHandler(IItemRepository IItemRepository) : ICommandHandler<DeleteItemsCommand>
{
    public async Task<Result> Handle(
        DeleteItemsCommand command,
        CancellationToken cancellationToken)
    {
        if(command.ItemIds.Count == 0) return Result.Success();

        bool inventoryExists = await IItemRepository.ExistsAsync(
            i => i.InventoryId == command.InventoryId,
            cancellationToken);
        if (!inventoryExists) return InventoryErrors.NotFound(command.InventoryId);

        await IItemRepository.BulkDeleteAsync(
            i => command.ItemIds.Contains(i.Id),
            cancellationToken);
            
        return Result.Success();
    }
}
