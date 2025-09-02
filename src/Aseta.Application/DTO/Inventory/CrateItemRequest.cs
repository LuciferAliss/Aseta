namespace Aseta.Application.DTO.Inventory;

public record CrateItemRequest(Guid InventoryId, List<CustomFieldValueRequest> CustomFields);