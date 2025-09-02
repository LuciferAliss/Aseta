namespace Aseta.Application.DTO.Inventory;

public record UpdateCustomFieldsRequest(Guid InventoryId, List<UpdateCustomFieldDefinitionRequest> CustomFields);