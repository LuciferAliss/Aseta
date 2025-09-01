using Aseta.Domain.Entities.Items;

namespace Aseta.Application.DTO.Inventory;

public record UpdateItemRequest(Guid ItemId, Guid InventoryId, List<CustomField> CustomFields);