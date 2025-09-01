using Aseta.Application.Abstractions.Services;
using Aseta.Application.DTO.Inventory;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Application.Services;

public class InventoryService(IInventoryRepository inventoryRepository,
    IItemRepository itemRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    ICustomIdService customIdService,
    UserManager<UserApplication> userManager) : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository = inventoryRepository;
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IInventoryUserRoleRepository _inventoryUserRoleRepository = inventoryUserRoleRepository;
    private readonly ICustomIdService _customIdService = customIdService;
    private readonly UserManager<UserApplication> _userManager = userManager;

    public async Task RemoveInventoryAsync(RemoveInventoryRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        await _inventoryRepository.DeleteAsync(inventory);
        await _inventoryRepository.SaveChangesAsync();
    }

    public async Task CreateInventoryAsync(CreateInventoryRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.CreatorId.ToString())
            ?? throw new Exception("User not found");

        var inventory = Inventory.Create(request.Name, request.CategoryId, user.Id);

        await _inventoryRepository.AddAsync(inventory);

        await _inventoryUserRoleRepository.AddAsync(InventoryUserRole.Create(user.Id, inventory.Id, InventoryRole.Owner));

        await _inventoryUserRoleRepository.SaveChangesAsync();
    }

    public async Task AddItemAsync(CrateItemRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        string customId = _customIdService.GenerateAsync(inventory.CustomIdParts);

        var item = Item.Create(customId, inventory.Id, request.CreatorId, request.CustomFields);

        await _itemRepository.AddAsync(item);
        await _itemRepository.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(RemoveItemRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        var item = await _itemRepository.GetByIdAsync(request.ItemId)
            ?? throw new Exception("Item not found");

        if (item.InventoryId != inventory.Id)
            throw new Exception("Item not found in inventory");

        await _itemRepository.DeleteAsync(item);
        await _itemRepository.SaveChangesAsync();
    }
}