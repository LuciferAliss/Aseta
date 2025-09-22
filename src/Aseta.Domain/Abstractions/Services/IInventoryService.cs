using Aseta.Domain.DTO;
using Aseta.Domain.DTO.Category;
using Aseta.Domain.DTO.CustomField;
using Aseta.Domain.DTO.CustomId;
using Aseta.Domain.DTO.Inventory;
using Aseta.Domain.DTO.Tag;

namespace Aseta.Domain.Abstractions.Services;

public interface IInventoryService
{
    Task<string> GetUserRoleInventoryAsync(Guid inventoryId, Guid userId);
    Task<CollectionResponse<CategoryResponse>> GetAllCategoryAsync();
    Task<PaginatedResult<ViewInventoryResponse>> GetLastInventoriesAsync(ViewLatestInventoryRequest request);
    Task<CollectionResponse<ViewInventoryResponse>> GetMostPopularInventoriesAsync(int count);
    Task<InventoryResponse> GetInventoryAsync(Guid inventoryId);
    Task<CreatedInventoryResponse> CreateInventoryAsync(CreateInventoryRequest request, Guid userId);
    Task UpdateInventoryAsync(UpdateInventoryRequest request);
    Task RemoveInventoryAsync(Guid inventoryId);
    Task UpdateCategoryToInventoryAsync(UpdateInventoryCategoryRequest request);
    Task UpdateCustomFieldsToInventoryAsync(UpdateCustomFieldsRequest request);
    Task UpdateCustomIdRulePartsToInventoryAsync(UpdateCustomIdPartsRequest request, Guid inventoryId);
    Task UpdateTagsToInventoryAsync(UpdateInventoryTagsRequest request);
    Task<CollectionResponse<TagResponse>> GetTagsCloudAsync();
}