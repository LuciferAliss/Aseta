using Aseta.Domain.DTO.CustomField;
using Aseta.Domain.DTO.User;

namespace Aseta.Domain.DTO.Item;

public record ItemResponse
{
    public Guid Id { get; init; }
    public string CustomId { get; init; }
    public List<CustomFieldValueResponse> CustomFields { get; init; } 
    public UserInventoryInfoResponse UserUpdate { get; init; }
    public UserInventoryInfoResponse UserCreate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
};