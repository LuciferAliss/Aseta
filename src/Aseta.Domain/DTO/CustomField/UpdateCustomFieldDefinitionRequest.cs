using Aseta.Domain.Entities.CustomField;

namespace Aseta.Domain.DTO.CustomField;

public record UpdateCustomFieldDefinitionRequest
(
    Guid? Id,
    string Name,
    CustomFieldType Type
);