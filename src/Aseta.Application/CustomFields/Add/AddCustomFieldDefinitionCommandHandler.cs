using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.Application.CustomFields.Add;

internal sealed class AddCustomFieldDefinitionCommandHandler(
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddCustomFieldDefinitionCommand>
{
    public async Task<Result> Handle(
        AddCustomFieldDefinitionCommand command,
        CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        var customFIeldResults = command.NewFields.Select(cfd => CustomFieldDefinition.Create(cfd.Name, cfd.Type)).ToList();

        if (customFIeldResults.Any(r => r.IsFailure))
        {
            return customFIeldResults.First(r => r.IsFailure).Error;
        }

        var customFields = customFIeldResults.Select(r => r.Value).ToList();

        Result result = inventory.AddCustomFields(customFields);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
