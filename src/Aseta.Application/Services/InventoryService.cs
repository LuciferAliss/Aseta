using Aseta.Application.Abstractions.Services;
using Aseta.Application.DTO;
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
using AutoMapper.Configuration.Annotations;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Application.Services;

public class InventoryService(
    IInventoryRepository inventoryRepository,
    IItemRepository itemRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    ICustomIdService customIdService,
    ITagRepository tagRepository,
    ICategoryRepository categoryRepository,
    UserManager<UserApplication> userManager,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IInventoryService
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

    public async Task<CollectionResponse<CategoryResponse>> GetAllCategoryAsync()
    {
        var category = await _categoryRepository.GetAllAsync()
            ?? throw new Exception("Category not found");

        var response = _mapper.Map<List<CategoryResponse>>(category);

        return new CollectionResponse<CategoryResponse>(response);
    }

    public async Task<PaginatedResult<ViewInventoryResponse>> GetLastInventoriesAsync(ViewLatestInventoryRequest request)
    {
        int totalCount = await _inventoryRepository.CountAsync();

        var inventories = await _inventoryRepository.GetLastInventoriesPageAsync(request.PageNumber, request.PageSize);

        var items = _mapper.Map<List<ViewInventoryResponse>>(inventories);

        return new PaginatedResult<ViewInventoryResponse>(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount,
            request.PageNumber * request.PageSize < totalCount
        );
    }

    public async Task<CollectionResponse<ViewInventoryResponse>> GetMostPopularInventoriesAsync(int count)
    {
        var inventories = await _inventoryRepository.GetMostPopularInventoriesAsync(count);

        var items = _mapper.Map<List<ViewInventoryResponse>>(inventories);

        return new CollectionResponse<ViewInventoryResponse>(items);
    }

    public async Task<InventoryResponse> GetInventoryAsync(Guid inventoryId)
    {
        var inventoryQuery = _inventoryRepository.GetQueryable();

        var inventoryEntity = await inventoryQuery
            .Where(i => i.Id == inventoryId)
            .Include(i => i.Creator)
            .Include(i => i.Category)
            .Include(i => i.Tags)
            .FirstOrDefaultAsync()
            ?? throw new Exception("Inventory not found");

        var response = _mapper.Map<InventoryResponse>(inventoryEntity);

        return response;
    }

    public async Task RemoveInventoryAsync(Guid inventoryId)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(inventoryId)
            ?? throw new Exception("Inventory not found");

        await _inventoryRepository.DeleteAsync(inventory);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CreatedInventoryResponse> CreateInventoryAsync(CreateInventoryRequest request, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        var inventory = Inventory.Create(request.Name, request.Description, request.ImageUrl, request.CategoryId, user.Id, request.IsPublic);

        await _inventoryRepository.AddAsync(inventory);

        await _inventoryUserRoleRepository.AddAsync(InventoryUserRole.Create(user.Id, inventory.Id, InventoryRole.Owner));

        await _unitOfWork.SaveChangesAsync();

        return new CreatedInventoryResponse(inventory.Id);
    }

    public async Task UpdateTagsToInventoryAsync(UpdateInventoryTagsRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdWithTagsAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        if (request.Tags == null || request.Tags.Count == 0)
        {
            inventory.UpdateTags([]);
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        var requestedTag = request.Tags
            .Distinct()
            .Select(t => t.Name)
            .ToList();

        var existingTags = await _tagRepository.GetByNamesAsync(requestedTag);
        var existingTagNames = existingTags.Select(t => t.Name.ToLowerInvariant()).ToList();

        var newTagNames = requestedTag.Except(existingTagNames);

        var newTags = newTagNames.Select(Tag.Create).ToList();
        if (newTags.Count != 0)
        {
            await _tagRepository.AddTagsAsync(newTags);
        }

        var allTagsForInventory = existingTags.Concat(newTags).ToList();
        inventory.UpdateTags(allTagsForInventory);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCustomFieldsToInventoryAsync(UpdateCustomFieldsRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        var oldFieldIds = inventory.CustomFields.Select(f => f.Id).ToHashSet();
        var newFieldIdsFromRequest = request.CustomFields
                                    .Where(f => f.Id.HasValue)
                                    .Select(f => f.Id!.Value)
                                    .ToHashSet();

        var deletedFieldIds = oldFieldIds.Where(id => !newFieldIdsFromRequest.Contains(id)).ToList();

        if (deletedFieldIds.Count != 0)
        {
            await _inventoryRepository.DeleteByFieldIdsAsync(deletedFieldIds);
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

        var item = Item.Create(customId, inventory.Id, user.Id, customFieldValues);

        await _itemRepository.AddAsync(item);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveItemsAsync(RemoveItemsRequest request, Guid inventoryId)
    {
        if (!await _inventoryRepository.ExistsAsync(inventoryId))
        {
            throw new Exception("Inventory not found.");
        }

        await _itemRepository.DeleteByItemIdsAsync(request.ItemIds, inventoryId);
    }

    public async Task UpdateItemAsync(UpdateItemRequest request, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        var item = await _itemRepository.GetByIdAsync(request.ItemId)
            ?? throw new Exception("Item not found");

        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

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

    public async Task<PaginatedResult<ItemResponse>> GetItemAsync(ItemViewRequest request, Guid inventoryId)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(inventoryId)
            ?? throw new Exception("Inventory not found");

        int totalCount = await _itemRepository.CountItems(inventory.Id);

        var items = await _itemRepository.GetItemsPageAsync(inventory.Id, request.PageNumber, request.PageSize);

        List<ItemResponse> itemResponses = items.Select(i => _mapper.Map<ItemResponse>(i)).ToList();

        return new PaginatedResult<ItemResponse>(
            itemResponses,
            request.PageNumber,
            request.PageSize,
            totalCount,
            request.PageNumber * request.PageSize < totalCount);
    }

    public async Task UpdateInventoryAsync(UpdateInventoryRequest request)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        inventory.Update(request.Name, request.Description, request.ImageUrl, request.IsPublic);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CollectionResponse<TagResponse>> GetTagsCloudAsync()
    {
        var tags = _tagRepository.GetAllAsQueryable();

        var tagResponses = await tags
            .ProjectTo<TagResponse>(_mapper.ConfigurationProvider)
            .OrderByDescending(t => t.Weight)
            .Take(50)
            .ToListAsync();

        return new CollectionResponse<TagResponse>(tagResponses);
    }

    public async Task<string> GetUserRoleInventoryAsync(Guid inventoryId, Guid userId)
    {
        var roleEntity = await _inventoryUserRoleRepository.GetUserRoleInventoryAsync(inventoryId, userId);

        if (roleEntity == null)
        {
            return "Viewer"; 
        }

        return roleEntity.Role.ToString();
    }
}