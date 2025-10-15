using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
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
        var inventory = await inventoryRepository.FirstOrDefaultAsync(
            i => i.Id == command.InventoryId,
            cancellationToken);
        if (inventory is null) return InventoryErrors.NotFound(command.InventoryId);

        await inventoryRepository.DeleteAsync(inventory);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}