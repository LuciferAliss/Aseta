namespace Aseta.Application.Abstractions.Authorization;

public interface ICurrentUserService
{
    string? UserId { get; }
    bool IsAuthenticated { get; }
}