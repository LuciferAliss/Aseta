using System;
using Aseta.Application.DTO.Inventory;

namespace Aseta.Application.Abstractions.Services;

public interface IInventoryService
{
    Task CreateInventoryAsync(CreateInventoryRequest request, Guid userId);
    Task RemoveInventoryAsync(Guid inventoryId);
    Task AddItemAsync(CrateItemRequest request, Guid userId);
    Task RemoveItemAsync(RemoveItemRequest request);
    Task UpdateItemAsync(UpdateItemRequest request, Guid userId);
}
