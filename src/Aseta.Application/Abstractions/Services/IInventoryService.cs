using Aseta.Application.DTO;
using Aseta.Application.DTO.Category;
using Aseta.Application.DTO.CustomField;
using Aseta.Application.DTO.CustomId;
using Aseta.Application.DTO.Inventory;
using Aseta.Application.DTO.Item;
using Aseta.Application.DTO.Tag;

namespace Aseta.Application.Abstractions.Services;

public interface IInventoryService
{
    Task<string> GetUserRoleInventoryAsync(Guid inventoryId, Guid userId);
    Task<CollectionResponse<CategoryResponse>> GetAllCategoryAsync();
    Task<PaginatedResult<ViewInventoryResponse>> GetLastInventoriesAsync(ViewLatestInventoryRequest request);
    Task<CollectionResponse<ViewInventoryResponse>> GetMostPopularInventoriesAsync(int count);
    Task<InventoryResponse> GetInventoryAsync(Guid inventoryId);
    Task AddItemAsync(CrateItemRequest request, Guid userId);
    Task<CreatedInventoryResponse> CreateInventoryAsync(CreateInventoryRequest request, Guid userId);
    Task UpdateInventoryAsync(UpdateInventoryRequest request);
    Task RemoveInventoryAsync(Guid inventoryId);
    Task RemoveItemsAsync(RemoveItemsRequest request, Guid inventoryId);
    Task UpdateCategoryToInventoryAsync(UpdateInventoryCategoryRequest request);
    Task UpdateCustomFieldsToInventoryAsync(UpdateCustomFieldsRequest request);
    Task UpdateCustomIdRulePartsToInventoryAsync(UpdateCustomIdPartsRequest request, Guid inventoryId);
    Task UpdateItemAsync(UpdateItemRequest request, Guid inventoryId, Guid userId);
    Task UpdateTagsToInventoryAsync(UpdateInventoryTagsRequest request);
    Task<PaginatedResult<ItemResponse>> GetItemAsync(ItemViewRequest request, Guid inventoryId);
    Task<CollectionResponse<TagResponse>> GetTagsCloudAsync();
}