namespace Aseta.Application.DTO.Inventory;

public record UpdateItemRequest(Guid ItemId, Guid InventoryId, string CustomId, List<CustomFieldValueRequest> CustomFields);