namespace Aseta.Application.DTO.Inventory;

public record UpdateInventoryTagsRequest(Guid InventoryId, List<AddTagsRequest> Tags);