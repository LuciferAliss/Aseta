using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.Domain.DTO.CustomField;

public record UpdateCustomFieldDefinitionRequest
(
    Guid? Id,
    string Name,
    CustomFieldType Type
);