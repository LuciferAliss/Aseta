using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.Items;
using AutoMapper;

namespace Aseta.Application.Items.Create;

internal sealed class CreateItemCommandHandler(
    IItemRepository itemRepository,
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICustomIdService customIdService) : ICommandHandler<CreateItemCommand>
{
    public async Task<Result> Handle(
        CreateItemCommand command,
        CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        Result<string> createResult = await customIdService.GenerateAsync(command.InventoryId, command.InventoryId, inventory.CustomIdRules, cancellationToken);
        if (createResult.IsFailure)
        {
            return createResult.Error;
        }

        ICollection<CustomFieldValue> customFieldsValue = mapper.Map<ICollection<CustomFieldValue>>(command.CustomFieldsValue);

        Result<Item> itemResult = Item.Create(
            createResult.Value,
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