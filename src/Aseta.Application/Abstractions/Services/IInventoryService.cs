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
    Task<CollectionResponse<CategoryResponse>> GetAllCategoryAsync();
    Task<PaginatedResult<ViewInventoryResponse>> GetLastInventoriesAsync(ViewLatestInventoryRequest request);
    Task<CollectionResponse<ViewInventoryResponse>> GetMostPopularInventoriesAsync(int count);
    Task<InventoryResponse> GetInventoryAsync(Guid inventoryId);
    Task AddItemAsync(CrateItemRequest request, Guid userId);
    Task<CreatedInventoryResponse> CreateInventoryAsync(CreateInventoryRequest request, Guid userId);
    Task UpdateInventoryAsync(UpdateInventoryRequest request);
    Task RemoveInventoryAsync(Guid inventoryId);
    Task RemoveItemAsync(RemoveItemRequest request);
    Task UpdateCategoryToInventoryAsync(UpdateInventoryCategoryRequest request);
    Task UpdateCustomFieldsToInventoryAsync(UpdateCustomFieldsRequest request);
    Task UpdateCustomIdRulePartsToInventoryAsync(UpdateCustomIdPartsRequest request);
    Task UpdateItemAsync(UpdateItemRequest request, Guid userId);
    Task UpdateTagsToInventoryAsync(UpdateInventoryTagsRequest request);
    Task<PaginatedResult<ItemResponse>> GetItemAsync(ItemViewRequest request, Guid UserId);
    Task<CollectionResponse<TagResponse>> GetTagsCloudAsync();
}
