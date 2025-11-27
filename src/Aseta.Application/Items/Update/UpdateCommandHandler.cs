using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;

namespace Aseta.Application.Items.Update;

internal sealed class UpdateCommandHandler(
    IItemRepository itemRepository,
    IInventoryRepository inventoryRepository,
    ICustomIdService customIdService,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateCommand>
{
    public async Task<Result> Handle(
        UpdateCommand command,
        CancellationToken cancellationToken)
    {
        var item = await itemRepository.GetByIdAsync(
            command.ItemId,
            true,
            cancellationToken);
        if (item is null) return ItemErrors.NotFound(command.ItemId);

        var inventory = await inventoryRepository.GetByIdAsync(
            command.InventoryId,
            false,
            cancellationToken);
        if (inventory is null) return InventoryErrors.NotFound(command.InventoryId);

        var customIdResult = await DetermineNewCustomIdAsync(
            command.CustomId,
            item, inventory);
        if (customIdResult.IsFailure) return Result.Failure(customIdResult.Error);

        item.Update(
            command.UserId,
            customIdResult.Value,
            command.CustomFieldsValue);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Result<string>> DetermineNewCustomIdAsync(
        string requestedCustomId,
        Item currentItem,
        Inventory inventory
    )
    {
        var customIdValidationResult = customIdService
            .IsValid(requestedCustomId, inventory.CustomIdRules);

        if (requestedCustomId != currentItem.CustomId)
        {
            return customIdValidationResult.IsSuccess
                ? requestedCustomId
                : customIdValidationResult.Error;
        }
        
        if (customIdValidationResult.IsSuccess)
        {
            return currentItem.CustomId;
        }
        else
        {
            return await customIdService.GenerateAsync(
                inventory.CustomIdRules,
                currentItem.InventoryId);
        }
    }
}
