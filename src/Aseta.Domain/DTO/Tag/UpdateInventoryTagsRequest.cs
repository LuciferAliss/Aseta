namespace Aseta.Domain.DTO.Tag;

public record UpdateInventoryTagsRequest(Guid InventoryId, List<AddTagsRequest> Tags);