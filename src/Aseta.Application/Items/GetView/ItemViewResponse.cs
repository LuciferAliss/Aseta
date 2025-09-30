using Aseta.Domain.Entities.CustomField;

namespace Aseta.Application.Items.GetView;

public record ItemViewResponse
{
    public Guid Id { get; init; }
    public required string CustomId { get; init; }
    public required List<CustomFieldValue> CustomFields { get; init; } 
    public required string UpdaterName { get; init; }
    public required string CreatorName { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
};