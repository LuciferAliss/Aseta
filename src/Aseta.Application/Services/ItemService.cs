using Aseta.Application.Items.GetItems;
using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.DTO.Item;
using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Application.Services;

public interface IItemService
{
    Task<Result> AddItemAsync(CrateItemRequest request, Guid inventoryId, Guid userId);
    Task<Result> RemoveItemsAsync(RemoveItemsRequest request, Guid inventoryId);
    Task<Result> UpdateItemAsync(UpdateItemRequest request, Guid inventoryId, Guid userId);
    Task<Result<PaginatedResult<ItemResponse>>> GetItemAsync(ItemViewRequest request, Guid inventoryId);
}


public class ItemService(
    IItemRepository itemRepository,
    IInventoryRepository inventoryRepository,
    UserManager<UserApplication> userManager,
    ICustomIdService customIdService,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IItemService
{
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IInventoryRepository _inventoryRepository = inventoryRepository;
    private readonly UserManager<UserApplication> _userManager = userManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICustomIdService _customIdService = customIdService;
    private readonly IMapper _mapper = mapper;

    public async Task<Result> DeleteItemsAsync(
        RemoveItemsRequest request,
        Guid inventoryId
    )
    {
        var result = await InventoryExistsAsync(inventoryId);
        if (result.IsFailure) return result.Error;

        await _itemRepository.DeleteByItemIdsAsync(request.ItemIds, inventoryId);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> UpdateItemAsync(
        UpdateItemRequest request,
        Guid inventoryId, Guid userId
    )
    {
        var userResult = await GetUserByIdAsync(userId);
        if (userResult.IsFailure)
            return userResult.Error;

        var inventoryResult = await GetInventoryByIdAsync(inventoryId);
        if (inventoryResult.IsFailure)
            return inventoryResult.Error;

        var itemResult = await GetItemByIdAsync(request.ItemId);
        if (itemResult.IsFailure)
            return itemResult.Error;

        var user = userResult.Value;
        var item = itemResult.Value;
        var inventory = inventoryResult.Value;

        var customIdResult = await DetermineNewCustomIdAsync(request.CustomId, item, inventory);
        if (customIdResult.IsFailure)
            return customIdResult.Error;

        var customId = customIdResult.Value;

        var customFieldValues = _mapper.Map<List<CustomFieldValue>>(
            inventory.CustomFields,
            opts => opts.Items["requestedFields"] = request.CustomFields
        );

        item.Update(customId, user.Id, customFieldValues);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<PaginatedResult<ItemResponse>>> GetItemAsync(
        ItemViewRequest request,
        Guid inventoryId
    )
    {
        var result = await InventoryExistsAsync(inventoryId);
        if (result.IsFailure) return result.Error;

        int totalCount = await _itemRepository.CountItems(inventoryId);

        var items = await _itemRepository.GetItemsPageAsync(
            inventoryId,
            request.PageNumber,
            request.PageSize
        );

        var itemResponses = items.Select(_mapper.Map<ItemResponse>).ToList();

        return new PaginatedResult<ItemResponse>(
            itemResponses,
            request.PageNumber,
            request.PageSize,
            totalCount,
            request.PageNumber * request.PageSize < totalCount
        );
    }

    private async Task<Result<UserApplication>> GetUserByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user is not null ? user : ItemServiceErrors.NotFoundUser;
    }

    private async Task<Result<Item>> GetItemByIdAsync(Guid itemId)
    {
        var item = await _itemRepository.GetByIdAsync(itemId);
        return item is not null ? item : ItemServiceErrors.NotFoundItem;
    }

    private async Task<Result<Inventory>> GetInventoryByIdAsync(Guid inventoryId)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(inventoryId);
        return inventory is not null ? inventory : ItemServiceErrors.NotFoundInventory;
    }

    private async Task<Result<bool>> InventoryExistsAsync(Guid inventoryId)
    {
        return await _inventoryRepository.ExistsAsync(inventoryId)
            ? true 
            : ItemServiceErrors.NotExistingInventory;
    }
    
    private async Task<Result<string>> DetermineNewCustomIdAsync(
        string requestedCustomId,
        Item currentItem,
        Inventory inventory
    )
    {
        var customIdValidationResult = _customIdService
            .IsValid(requestedCustomId, inventory.CustomIdRules);

        if (requestedCustomId != currentItem.CustomId)
        {
            return customIdValidationResult.IsSuccess
                ? requestedCustomId
                : customIdValidationResult.Error;
        }
        
        if (customIdValidationResult.IsSuccess)
        {
            return currentItem.CustomId;
        }
        else
        {
            return await _customIdService
                .GenerateAsync(inventory.CustomIdRules, currentItem.InventoryId);
        }
    }
}
