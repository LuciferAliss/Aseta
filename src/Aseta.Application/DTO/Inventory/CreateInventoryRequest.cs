namespace Aseta.Application.DTO.Inventory;

public record CreateInventoryRequest
(
    string Name,
    string Description,
    string ImageUrl,
    bool IsPublic,
    Guid CreatorId,
    int CategoryId
);