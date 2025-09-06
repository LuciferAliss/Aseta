namespace Aseta.Application.DTO.Item;

public record ItemViewRequest(Guid InventoryId, int PageNumber, int PageSize);