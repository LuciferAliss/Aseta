namespace Aseta.Application.Abstractions.Authorization;

public interface IUserContext
{
    string? UserId { get; }
    bool IsAuthenticated { get; }
}