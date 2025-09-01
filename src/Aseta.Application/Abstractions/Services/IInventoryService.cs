using System;
using Aseta.Application.DTO.Inventory;

namespace Aseta.Application.Abstractions.Services;

public interface IInventoryService
{
    Task CreateInventoryAsync(CreateInventoryRequest request);
    Task RemoveInventoryAsync(RemoveInventoryRequest request);
    Task AddItemAsync(CrateItemRequest request);
    Task RemoveItemAsync(RemoveItemRequest request);
}
