using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.DTO;
using Aseta.Domain.DTO.Category;
using Aseta.Domain.DTO.CustomField;
using Aseta.Domain.DTO.CustomId;
using Aseta.Domain.DTO.Inventory;
using Aseta.Domain.DTO.Tag;
using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Inventories.CustomId;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.UserRoles;
using Aseta.Domain.Entities.Users;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Application.Services;

public class InventoryService(
    IInventoryRepository inventoryRepository,
    IItemRepository itemRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    ITagRepository tagRepository,
    ICategoryRepository categoryRepository,
    UserManager<ApplicationUser> userManager,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository = inventoryRepository;
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IInventoryUserRoleRepository _inventoryUserRoleRepository = inventoryUserRoleRepository;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
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

    public async Task<PaginatedResponse<ViewInventoryResponse>> GetLastInventoriesAsync(ViewLatestInventoryRequest request)
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
        var inventory = await _inventoryRepository.FirstOrDefaultAsync(inventoryId)
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

        await _inventoryUserRoleRepository.AddAsync(InventoryRole.Create(user.Id, inventory.Id, Role.Owner));

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
        var inventory = await _inventoryRepository.FirstOrDefaultAsync(request.InventoryId)
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
            return c.Id == Guid.Empty
                ? CustomFieldDefinition.Create(c.Name, c.Type)
                : CustomFieldDefinition.Create(c.Id, c.Name, c.Type);
        }).ToList();

        inventory.UpdateCustomFields(newCustomFields);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCustomIdRulePartsToInventoryAsync(UpdateCustomIdPartsRequest request, Guid inventoryId)
    {
        var inventory = await _inventoryRepository.FirstOrDefaultAsync(inventoryId)
            ?? throw new Exception("Inventory not found");

        var domainRules = _mapper.Map<List<CustomIdRuleBase>>(request.CustomIdRuleParts);

        inventory.UpdateCustomIdRuleParts(domainRules);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCategoryToInventoryAsync(UpdateInventoryCategoryRequest request)
    {
        var inventory = await _inventoryRepository.FirstOrDefaultAsync(request.InventoryId)
            ?? throw new Exception("Inventory not found");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
            ?? throw new Exception("Category not found");

        inventory.UpdateCategory(category.Id);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateInventoryAsync(UpdateInventoryRequest request)
    {
        var inventory = await _inventoryRepository.FirstOrDefaultAsync(request.InventoryId)
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