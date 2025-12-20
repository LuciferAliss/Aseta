using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.Application.CustomFields.Update;

internal sealed class UpdateCustomFieldDefinitionCommandHandler(
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateCustomFieldDefinitionCommand>
{
    public async Task<Result> Handle(
        UpdateCustomFieldDefinitionCommand command,
        CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        var existingIds = inventory.CustomFields.Select(cf => cf.Id).ToList();

        var missingIds = command.CustomFields
            .Select(val => val.FieldId)
            .Except(existingIds)
            .ToList();

        if (missingIds.Count > 0)
        {
            return CustomFieldErrors.CannotUpdateNonExistentField(missingIds);
        }

        var customFieldResults = command.CustomFields.Select(val => CustomFieldDefinition.Reconstitute(val.FieldId, val.Name, val.Type)).ToList();

        if (customFieldResults.Any(r => r.IsFailure))
        {
            return customFieldResults.First(r => r.IsFailure).Error;
        }

        var customFields = customFieldResults.Select(val => val.Value).ToList();

        Result result = inventory.UpdateCustomFields(customFields);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
