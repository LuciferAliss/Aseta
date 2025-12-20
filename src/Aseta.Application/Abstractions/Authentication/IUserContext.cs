using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Abstractions.Authentication;

public interface IUserContext
{
    string? UserId { get; }
    UserRole? UserRole { get; }
    bool IsAuthenticated { get; }
    string? SessionId { get; }
}