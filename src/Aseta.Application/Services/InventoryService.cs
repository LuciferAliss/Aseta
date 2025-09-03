using Aseta.Application.Abstractions.Services;
using Aseta.Application.DTO.Inventory;
using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Application.Services;

public class InventoryService(IInventoryRepository inventoryRepository,
    IItemRepository itemRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    ICustomIdService customIdService,
    ITagRepository tagRepository,
    ICategoryRepository categoryRepository,
    UserManager<UserApplication> userManager,
    IUnitOfWork unitOfWork) : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository = inventoryRepository;
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IInventoryUserRoleRepository _inventoryUserRoleRepository = inventoryUserRoleRepository;
    private readonly ICustomIdService _customIdService = customIdService;
    private readonly UserManager<UserApplication> _userManager = userManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task RemoveInventoryAsync(Guid inventoryId)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(inventoryId)
            ?? throw new Exception("Inventory not found");

        await _inventoryRepository.DeleteAsync(inventory);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CreateInventoryAsync(CreateInventoryRequest request, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        var inventory = Inventory.Create(request.Name, request.Description, request.ImageUrl, request.CategoryId, user.Id, request.IsPublic);

        await _inventoryRepository.AddAsync(inventory);

        await _inventoryUserRoleRepository.AddAsync(InventoryUserRole.Create(user.Id, inventory.Id, InventoryRole.Owner));

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateTagsToInventoryAsync(UpdateInventoryTagsRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        var tags = request.Tags.Select(t => Tag.Create(t.Name)).ToList();

        var checkTasks = tags.Select(async t => new { Tag = t, Exists = await _tagRepository.ExistsByNameAsync(t.Name) }).ToList();
        var results = await Task.WhenAll(checkTasks);
        var notExistingTags = results.Where(r => !r.Exists).Select(r => r.Tag).ToList();

        foreach (var tag in notExistingTags)
        {
            await _tagRepository.AddAsync(tag);
        }

        inventory.UpdateTags(tags);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCustomFieldsToInventoryAsync(UpdateCustomFieldsRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        var oldFieldIds = inventory.CustomFields.Select(f => f.Id).ToHashSet();
        var newFieldIdsFromRequest = request.CustomFields.Select(f => f.Id).ToHashSet();
        
        var deletedFieldIds = oldFieldIds.Where(id => !newFieldIdsFromRequest.Contains(id)).ToList();

        if (deletedFieldIds.Count != 0)
        {
            foreach (var item in inventory.Items)
            {
                item.CustomFieldValues.RemoveAll(value => deletedFieldIds.Contains(value.FieldId));
            }
        }

        var newCustomFields = request.CustomFields.Select(c => 
        {
            return (c.Id == Guid.Empty)
                ? CustomFieldDefinition.Create(c.Name, c.Description, c.Type, c.ShowInTableView)
                : CustomFieldDefinition.Create(c.Id, c.Name, c.Description, c.Type, c.ShowInTableView);
        }).ToList();

        inventory.UpdateCustomFields(newCustomFields);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCustomIdPartsToInventoryAsync(UpdateCustomIdPartsRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        // inventory.UpdateCustomIdParts(request.CustomIdParts);
        
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCategoryToInventoryAsync(UpdateInventoryCategoryRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
            ?? throw new Exception("Category not found");

        inventory.UpdateCategory(category.Id);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddItemAsync(CrateItemRequest request, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        // string customId = _customIdService.GenerateAsync(inventory.CustomIdParts);

        var customFieldValues = inventory.CustomFields.Select(d =>
        {
            var requestValue = request.CustomFields.FirstOrDefault(f => f.FieldId == d.Id);

            return new CustomFieldValue
            {
                FieldId = d.Id,
                Value = requestValue?.Value
            };
        }).ToList();

        var item = Item.Create("", inventory.Id, user.Id, customFieldValues);

        await _itemRepository.AddAsync(item);

        await _unitOfWork.SaveChangesAsync();
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

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateItemAsync(UpdateItemRequest request, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        var item = await _itemRepository.GetByIdAsync(request.ItemId)
            ?? throw new Exception("Item not found");

        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        if (item.InventoryId != inventory.Id)
            throw new Exception("Item not found in inventory");

        var customFieldValues = inventory.CustomFields.Select(d => 
        {
            var requestValue = request.CustomFields.FirstOrDefault(f => f.FieldId == d.Id);

            return new CustomFieldValue
            {
                FieldId = d.Id,
                Value = requestValue?.Value
            };
        }).ToList();

        item.Update(item.CustomId, user.Id, customFieldValues);

        await _unitOfWork.SaveChangesAsync();
    }
}