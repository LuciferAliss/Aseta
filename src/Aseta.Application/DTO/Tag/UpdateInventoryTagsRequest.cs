namespace Aseta.Application.DTO.Tag;

public record UpdateInventoryTagsRequest(Guid InventoryId, List<AddTagsRequest> Tags);