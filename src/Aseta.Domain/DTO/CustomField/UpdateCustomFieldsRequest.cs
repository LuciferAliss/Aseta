namespace Aseta.Domain.DTO.CustomField;

public record UpdateCustomFieldsRequest(Guid InventoryId, List<UpdateCustomFieldDefinitionRequest> CustomFields);