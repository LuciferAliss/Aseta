using Aseta.Application.DTO.CustomField;

namespace Aseta.Application.DTO.Item;

public record CrateItemRequest(Guid InventoryId, List<CustomFieldValueRequest> CustomFields);