namespace Aseta.Domain.DTO.Inventory;

public record CreateInventoryRequest
(
    string Name,
    string Description,
    string ImageUrl,
    bool IsPublic,
    int CategoryId
);