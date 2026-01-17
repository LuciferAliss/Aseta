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

        bool existingCustomField = inventory.CustomFields.Any(cf => cf.Id == command.FieldId);

        if (!existingCustomField)
        {
            return CustomFieldErrors.NotFound(command.FieldId);
        }

        Result result = inventory.UpdateCustomFields(command.FieldId, command.Name, command.Type);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
