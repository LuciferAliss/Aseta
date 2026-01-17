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

        CustomFieldDefinition? customField = inventory.CustomFields.FirstOrDefault(cf => cf.Id == command.FieldId);

        if (customField is null)
        {
            return CustomFieldErrors.NotFound(command.FieldId);
        }

        inventory.DeleteCustomField(customField);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
