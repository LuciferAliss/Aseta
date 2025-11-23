namespace Aseta.Application.Inventories.Get.Contracts;

public sealed record InventoryResponse
(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl,
    string Creator,
    CategoryResponse Category,
    bool IsPublic,
    DateTime CreatedAt,
    ICollection<TagResponse> Tags,
    ICollection<CustomFieldDefinitionResponse> CustomFieldsDefinition
);