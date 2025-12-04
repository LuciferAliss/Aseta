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
        Item item = await itemRepository.GetByIdAsync(
            command.ItemId,
            true,
            cancellationToken);

        if (item is null)
        {
            return ItemErrors.NotFound(command.ItemId);
        }

        Inventory inventory = await inventoryRepository.GetByIdAsync(
            command.InventoryId,
            false,
            cancellationToken);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        Result<string> customIdResult = await DetermineNewCustomIdAsync(
            command.CustomId,
            item,
            inventory,
            cancellationToken);

        if (customIdResult.IsFailure)
        {
            return Result.Failure(customIdResult.Error);
        }

        item.Update(
            command.UserId,
            customIdResult.Value,
            command.CustomFieldsValue);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Result<string>> DetermineNewCustomIdAsync(
        string requestedCustomId,
        Item item,
        Inventory inventory,
        CancellationToken cancellationToken = default)
    {
        Result customIdValidationResult = customIdService
            .IsValid(requestedCustomId, inventory.CustomIdRules);

        if (requestedCustomId != item.CustomId)
        {
            return customIdValidationResult.IsSuccess
                ? requestedCustomId
                : customIdValidationResult.Error;
        }

        if (customIdValidationResult.IsSuccess)
        {
            return item.CustomId;
        }
        else
        {
            return await customIdService.GenerateAsync(
                inventory.Id,
                item.Id,
                inventory.CustomIdRules,
                cancellationToken);
        }
    }
}
