using Aseta.Domain.DTO.CustomField;

namespace Aseta.Domain.DTO.Item;

public record CrateItemRequest(List<CustomFieldValueRequest> CustomFields);