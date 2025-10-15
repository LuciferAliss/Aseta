using Aseta.Application.Contracts;
using Aseta.Application.Contracts.CustomIdRule;

namespace Aseta.Application.Inventories.Get;

public sealed record InventoryResponse
(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl,
    string Creator,
    CategoryResponse? Category,
    bool IsPublic,
    DateTime CreatedAt,
    ICollection<TagResponse> Tags,
    ICollection<CustomFieldDefinitionResponse> CustomFieldsDefinition,
    ICollection<CustomIdRuleResponse> CustomIdRules
);