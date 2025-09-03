namespace Aseta.Application.DTO.CustomField;

public record UpdateCustomFieldsRequest(Guid InventoryId, List<UpdateCustomFieldDefinitionRequest> CustomFields);