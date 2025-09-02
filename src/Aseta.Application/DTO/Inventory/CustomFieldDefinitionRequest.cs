using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.DTO.Inventory;

public record CreateCustomFieldDefinitionRequest
(
    string Name,
    string Description,
    CustomFieldType Type,
    bool ShowInTableView
);

