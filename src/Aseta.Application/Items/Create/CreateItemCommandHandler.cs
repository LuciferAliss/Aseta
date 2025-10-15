using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
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
        var inventory = await inventoryRepository.FirstOrDefaultAsync(
            i => i.Id == command.InventoryId,
            cancellationToken);
        if (inventory is null) return InventoryErrors.NotFound(command.InventoryId);

        var customIdResult = await customIdService
            .GenerateAsync(inventory.CustomIdRules, command.InventoryId);
        if (customIdResult.IsFailure) return customIdResult;

        var item = Item.Create(
            customIdResult.Value,
            command.InventoryId,
            command.CustomFieldsValue,
            command.UserId
        );
        
        await itemRepository.AddAsync(item, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}