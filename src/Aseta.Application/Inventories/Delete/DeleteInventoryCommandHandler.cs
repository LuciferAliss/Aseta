using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.Inventories.Delete;

internal sealed class DeleteInventoryCommandHandler(
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteInventoryCommand>
{
    public async Task<Result> Handle(
        DeleteInventoryCommand command,
        CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        inventoryRepository.Remove(inventory);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}