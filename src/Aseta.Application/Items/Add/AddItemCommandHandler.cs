using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Users;
using Aseta.Domain.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Application.Items.Add;

internal sealed class AddItemCommandHandler(
    IItemRepository itemRepository,
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UserManager<UserApplication> userManager,
    ICustomIdService customIdService
) : ICommandHandler<AddItemCommand>
{
    public async Task<Result> Handle(AddItemCommand command, CancellationToken cancellationToken)
    {
        var userResult = await GetUserByIdAsync(command.UserId);
        if (userResult.IsFailure) return userResult;

        var inventoryResult = await GetInventoryByIdAsync(command.InventoryId);
        if (inventoryResult.IsFailure) return inventoryResult;

        var inventory = inventoryResult.Value;

        var customIdResult = await customIdService
            .GenerateAsync(inventory.CustomIdRules, command.InventoryId);
        if (customIdResult.IsFailure) return customIdResult;

        var customFieldValues = mapper.Map<List<CustomFieldValue>>(
            inventory.CustomFields,
            opts => opts.Items["requestedFields"] = command.CustomFields
        );

        var item = new Item
        {
            CustomId = customIdResult.Value,
            CustomFieldValues = customFieldValues,
            InventoryId = command.InventoryId,
            CreatorId = command.UserId 
        };

        await itemRepository.AddAsync(item);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Result<UserApplication>> GetUserByIdAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        return user is not null ? user : ItemServiceErrors.NotFoundUser;
    }

    private async Task<Result<Item>> GetItemByIdAsync(Guid itemId)
    {
        var item = await itemRepository.GetByIdAsync(itemId);
        return item is not null ? item : ItemServiceErrors.NotFoundItem;
    }

    private async Task<Result<Inventory>> GetInventoryByIdAsync(Guid inventoryId)
    {
        var inventory = await inventoryRepository.GetByIdAsync(inventoryId);
        return inventory is not null ? inventory : ItemServiceErrors.NotFoundInventory;
    }
}
