using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.Inventories.Update;

internal sealed class UpdateInventoryCommandHandler(
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateInventoryCommand>
{
    public async Task<Result> Handle(
        UpdateInventoryCommand command,
        CancellationToken cancellationToken)
    {
        var inventory = await inventoryRepository.GetByIdAsync(
            command.InventoryId,
            true,
            cancellationToken);
        if (inventory is null) return InventoryErrors.NotFound(command.InventoryId);

        inventory.Update(
            command.Name,
            command.Description,
            command.ImageUrl,
            command.IsPublic);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
