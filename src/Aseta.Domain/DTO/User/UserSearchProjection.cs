namespace Aseta.Domain.DTO.User;

public sealed record UserSearchProjection(
    Guid Id,
    string UserName,
    string Email,
    float Rank);
