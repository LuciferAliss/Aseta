namespace Aseta.Application.DTO.User;

public record UserAdminViewResponse(
    Guid Id,
    string Username,
    string Email,
    bool IsAdmin,
    bool IsBlocked
);