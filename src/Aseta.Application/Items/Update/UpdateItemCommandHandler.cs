using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.Items;

namespace Aseta.Application.Items.Update;

internal sealed class UpdateItemCommandHandler(
    IItemRepository itemRepository,
    IInventoryRepository inventoryRepository,
    ICustomIdService customIdService,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateItemCommand>
{
    public async Task<Result> Handle(
        UpdateItemCommand command,
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
            cancellationToken: cancellationToken);

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
            return customIdResult.Error;
        }

        string customId = customIdResult.Value;

        var existingIds = inventory.CustomFields.Select(cf => cf.Id).ToList();

        var missingIds = command.CustomFieldsValue
            .Select(val => val.FieldId)
            .Except(existingIds)
            .ToList();

        if (missingIds.Count > 0)
        {
            return CustomFieldErrors.NotFound(missingIds);
        }

        if (command.CustomFieldsValue.Count != existingIds.Count)
        {
            return CustomFieldErrors.AllFieldsRequired();
        }

        var customFieldDefinitionsMap = inventory.CustomFields.ToDictionary(d => d.Id, d => d);

        var customFieldResults = command.CustomFieldsValue
            .Select(c =>
            {
                if (!customFieldDefinitionsMap.TryGetValue(c.FieldId, out CustomFieldDefinition? fieldDefinition))
                {
                    return CustomFieldErrors.NotFound(c.FieldId);
                }

                return CustomFieldValue.Create(c.FieldId, c.Value, fieldDefinition.Type);
            })
            .ToList();

        if (customFieldResults.Any(r => r.IsFailure))
        {
            return customFieldResults.First(r => r.IsFailure).Error;
        }

        var customFieldValues = customFieldResults.Select(r => r.Value).ToList();

        Result updateResult = item.Update(
            command.UserId,
            customId,
            customFieldValues);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

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
