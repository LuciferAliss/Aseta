using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;

namespace Aseta.Application.Items.Create;

internal sealed class CrateItemCommandHandler(
    IItemRepository itemRepository,
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork,
    ICustomIdService customIdService
) : ICommandHandler<CreateItemCommand>
{
    public async Task<Result> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, cancellationToken);
        if (inventory is null) return Result.Failure(InventoryErrors.NotFound(command.InventoryId));

        var customIdResult = await customIdService
            .GenerateAsync(inventory.CustomIdRules, command.InventoryId);
        if (customIdResult.IsFailure) return customIdResult;

        var item = new Item(
            customIdResult.Value,
            command.InventoryId,
            command.CustomFieldsValue,
            command.UserId
        );
        
        await itemRepository.AddAsync(item, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}