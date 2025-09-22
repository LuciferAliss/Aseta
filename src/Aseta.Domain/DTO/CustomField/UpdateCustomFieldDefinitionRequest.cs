using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.DTO.CustomField;

public record UpdateCustomFieldDefinitionRequest
(
    Guid? Id,
    string Name,
    CustomFieldType Type
);