using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;

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
        var inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken);
        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        var customIdRules = await inventoryRepository.GetCustomIdRuleAsync(
            command.InventoryId, cancellationToken);
        
        var createResult = await customIdService.GenerateAsync(
            customIdRules, command.InventoryId, cancellationToken);
        if (createResult.IsFailure) return createResult;

        var itemResult = Item.Create(
            createResult.Value,
            command.InventoryId,
            command.CustomFieldsValue,
            command.UserId);
        if (itemResult.IsFailure) return itemResult.Error;

        await itemRepository.AddAsync(itemResult.Value, cancellationToken);
        
        inventory.IncrementItemsCount();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}