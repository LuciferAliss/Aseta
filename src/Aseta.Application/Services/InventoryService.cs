using Aseta.Application.Abstractions.Services;
using Aseta.Application.DTO.Category;
using Aseta.Application.DTO.CustomField;
using Aseta.Application.DTO.CustomId;
using Aseta.Application.DTO.Inventory;
using Aseta.Application.DTO.Item;
using Aseta.Application.DTO.Tag;
using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Application.Services;

public class InventoryService(IInventoryRepository inventoryRepository,
    IItemRepository itemRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    ICustomIdService customIdService,
    ITagRepository tagRepository,
    ICategoryRepository categoryRepository,
    UserManager<UserApplication> userManager,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository = inventoryRepository;
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IInventoryUserRoleRepository _inventoryUserRoleRepository = inventoryUserRoleRepository;
    private readonly ICustomIdService _customIdService = customIdService;
    private readonly UserManager<UserApplication> _userManager = userManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<InventoryResponse>> GetAllInventoriesInPublicAsync(Guid inventoryId, Guid userId)
    {
        Guid verifiedUserId;

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user != null) verifiedUserId = user.Id;
        else verifiedUserId = Guid.Empty;

        var inventories = await _inventoryRepository.GetAllPublicInventoriesAsync(verifiedUserId);

        List<InventoryResponse> inventoryResponses = _mapper.Map<List<InventoryResponse>>(inventories);

        return inventoryResponses;
    }

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
            var items = await _itemRepository.GetByItemsInventoryIdAsync(inventory.Id);
            foreach (var item in items)
            {
                item.CustomFieldValues.RemoveAll(value => deletedFieldIds.Contains(value.FieldId));
            }
        }

        var newCustomFields = request.CustomFields.Select(c =>
        {
            return (c.Id == Guid.Empty)
                ? CustomFieldDefinition.Create(c.Name, c.Type, c.ShowInTableView)
                : CustomFieldDefinition.Create(c.Id, c.Name, c.Type, c.ShowInTableView);
        }).ToList();

        inventory.UpdateCustomFields(newCustomFields);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCustomIdRulePartsToInventoryAsync(UpdateCustomIdPartsRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        inventory.UpdateCustomIdRuleParts(request.CustomIdParts);

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

        string customId = _customIdService.GenerateAsync(inventory.CustomIdRules, inventory.Id);

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

        string customId = "";

        if (request.CustomId != item.CustomId)
        {
            if (!_customIdService.IsValid(request.CustomId, inventory.CustomIdRules))
                throw new Exception("Invalid custom id");
            else
                customId = request.CustomId;
        }
        else
        {
            if (!_customIdService.IsValid(request.CustomId, inventory.CustomIdRules))
                customId = _customIdService.GenerateAsync(inventory.CustomIdRules, item.InventoryId);
            else
                customId = item.CustomId;
        }

        var customFieldValues = inventory.CustomFields.Select(d =>
        {
            var requestValue = request.CustomFields.FirstOrDefault(f => f.FieldId == d.Id);

            return new CustomFieldValue
            {
                FieldId = d.Id,
                Value = requestValue?.Value
            };
        }).ToList();

        item.Update(customId, user.Id, customFieldValues);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task GetItemAsync(Guid itemId)
    {
        var item = await _itemRepository.GetByIdAsync(itemId)
            ?? throw new Exception("Item not found");
    }

    public async Task UpdateInventoryAsync(UpdateInventoryRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        inventory.Update(request.Name, request.Description, request.ImageUrl, request.IsPublic);

        await _unitOfWork.SaveChangesAsync();
    }
}

public record InventoryResponse
(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl,
    CategoryResponse? Category,
    List<TagResponse> Tags
);

public record CustomIdRulePartResponse(string Type, string Value);

public record CustomFieldDefinitionResponse
(
    Guid Id, 
    string Name,
    string Type,
    bool ShowInTableView
);

public record TagResponse(int Id, string Name);

public record CategoryResponse(int Id, string Name);