namespace Aseta.Application.DTO.Inventory;

public record InventoryInfoProfileResponse
(
    Guid Id,
    string Name,
    bool IsPublic,
    string ImageUrl
);