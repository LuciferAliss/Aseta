using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;

namespace Aseta.Application.Items.Delete;

internal sealed class DeleteItemsCommandHandler(
    IItemRepository itemRepository,
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteItemsCommand>
{
    public async Task<Result> Handle(
        DeleteItemsCommand command,
        CancellationToken cancellationToken)
    {
        if (command.ItemIds.Count == 0)
        {
            return Result.Success();
        }

        await using ITransactionScope transaction = await unitOfWork.BeginTransactionScopeAsync(cancellationToken);

        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken);
        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        int deletedCount = await itemRepository.BulkRemoveAsync(
            item => item.InventoryId == command.InventoryId && command.ItemIds.Contains(item.Id),
            cancellationToken);

        if (deletedCount != command.ItemIds.Count)
        {
            return ItemErrors.DeletionFailed();
        }

        inventory.DecrementItemsCount(deletedCount);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
