using Aseta.Application.DTO.User;

namespace Aseta.Application.DTO.Inventory;

public record ViewInventoryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public UserInventoryInfoResponse Creator { get; init; }
    public bool IsPublic { get; init; }
};