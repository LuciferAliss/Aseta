using Aseta.Domain.DTO.CustomField;

namespace Aseta.Domain.DTO.Item;

public record UpdateItemRequest(Guid ItemId, string CustomId, List<CustomFieldValueRequest> CustomFields);