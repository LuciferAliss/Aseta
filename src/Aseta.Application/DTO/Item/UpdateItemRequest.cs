using Aseta.Application.DTO.CustomField;

namespace Aseta.Application.DTO.Item;

public record UpdateItemRequest(Guid ItemId, string CustomId, List<CustomFieldValueRequest> CustomFields);