using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.Items;
using AutoMapper;
using System.Linq;

namespace Aseta.Application.Items.Create;

internal sealed class CreateItemCommandHandler(
    IItemRepository itemRepository,
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork,
    ICustomIdService customIdService) : ICommandHandler<CreateItemCommand>
{
    public async Task<Result> Handle(
        CreateItemCommand command,
        CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(
            command.InventoryId,
            true,
            cancellationToken);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

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

        ICollection<CustomFieldValue> customFieldsValue = customFieldResults
            .Select(r => r.Value)
            .ToList();

        Result<string> customIdResult = await customIdService.GenerateAsync(
            command.InventoryId,
            command.InventoryId,
            inventory.CustomIdRules,
            cancellationToken);

        if (customIdResult.IsFailure)
        {
            return customIdResult.Error;
        }

        Result<Item> itemResult = Item.Create(
            customIdResult.Value,
            command.InventoryId,
            customFieldsValue,
            command.UserId);

        if (itemResult.IsFailure)
        {
            return itemResult.Error;
        }

        Item item = itemResult.Value;

        await itemRepository.AddAsync(item, cancellationToken);

        inventory.IncrementItemsCount();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}