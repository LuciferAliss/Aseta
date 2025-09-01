namespace Aseta.Application.DTO.Inventory;

public record RemoveItemRequest(Guid ItemId, Guid InventoryId);