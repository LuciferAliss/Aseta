namespace Aseta.Application.DTO.Inventory;

public record UpdateInventoryRequest(Guid InventoryId, string Name, string Description, string ImageUrl, bool IsPublic);