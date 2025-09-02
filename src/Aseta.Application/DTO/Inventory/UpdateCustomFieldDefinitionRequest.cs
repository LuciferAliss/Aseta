using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.DTO.Inventory;

public record UpdateCustomFieldDefinitionRequest
(
    Guid Id,
    string Name,
    string Description,
    CustomFieldType Type,
    bool ShowInTableView
);

