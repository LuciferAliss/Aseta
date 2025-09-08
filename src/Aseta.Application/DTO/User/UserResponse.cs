namespace Aseta.Application.DTO.User;

public record UserResponse
(
    Guid Id,
    string Email,
    string Role
);