using Aseta.Application.DTO.Category;
using Aseta.Application.DTO.Tag;
using Aseta.Application.DTO.User;

namespace Aseta.Application.DTO.Inventory;

public record InventoryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
    public UserInventoryInfoResponse UserCreator { get; init; }
    public CategoryResponse? Category { get; init; }
    public List<TagResponse> Tags { get; init; }
};