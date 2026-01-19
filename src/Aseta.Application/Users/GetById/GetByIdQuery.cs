using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Users.GetById;

public sealed record GetByIdQuery(Guid Id) : IQuery<UserResponse>;

public record UserResponse(
    Guid Id,
    string UserName,
    string Email,
    bool IsLocked,
    string Role,
    DateTime CreatedAt,
    ICollection<InventoryResponse> Inventories);

public record InventoryResponse(
    Guid Id,
    string Name,
    Uri? ImageUrl,
    DateTime CreatedAt);