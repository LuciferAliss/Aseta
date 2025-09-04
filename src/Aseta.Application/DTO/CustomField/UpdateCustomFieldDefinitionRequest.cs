using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.DTO.CustomField;

public record UpdateCustomFieldDefinitionRequest
(
    Guid Id,
    string Name,
    CustomFieldType Type,
    bool ShowInTableView
);

