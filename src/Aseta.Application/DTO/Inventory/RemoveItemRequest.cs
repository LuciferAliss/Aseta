namespace Aseta.Application.DTO.Inventory;

public record RemoveItemRequest(Guid InventoryId, Guid ItemId);