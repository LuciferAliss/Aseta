using Aseta.Domain.DTO.Tag;
using Aseta.Domain.DTO.User;
using Aseta.Domain.DTO.Category;
using Aseta.Domain.DTO.CustomField;
using Aseta.Domain.DTO.CustomId;

namespace Aseta.Domain.DTO.Inventory;

public record InventoryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
    public UserInventoryInfoResponse UserCreator { get; init; }
    public CategoryResponse? Category { get; init; }
    public List<TagResponse> Tags { get; init; }
    public bool IsPublic { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<CustomFieldDefinitionResponse> CustomFieldsDefinition { get; init; }
    public List<CustomIdRulePartResponse> CustomIdRules { get; init; }
};