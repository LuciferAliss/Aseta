using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.Application.CustomFields.Delete;

public class DeleteCustomFieldDefinitionCommandHandler(
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteCustomFieldDefinitionCommand>
{
    public async Task<Result> Handle(
        DeleteCustomFieldDefinitionCommand command,
        CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken: cancellationToken);

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
            return CustomFieldErrors.CannotDeleteNonExistentField(missingIds);
        }

        var deleteCustomFieldsResult = command.CustomFields.Select(cfs => CustomFieldDefinition.Reconstitute(
            cfs.FieldId,
            cfs.Name,
            cfs.Type)).ToList();

        if (deleteCustomFieldsResult.Any(r => r.IsFailure))
        {
            return deleteCustomFieldsResult.First(r => r.IsFailure).Error;
        }

        var customFields = deleteCustomFieldsResult.Select(r => r.Value).ToList();

        Result deleteResult = inventory.DeleteCustomFields(customFields);

        if (deleteResult.IsFailure)
        {
            return deleteResult.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
