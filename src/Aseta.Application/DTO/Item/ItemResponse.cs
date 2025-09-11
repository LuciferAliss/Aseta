using Aseta.Application.DTO.CustomField;
using Aseta.Application.DTO.User;

namespace Aseta.Application.DTO.Item;

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