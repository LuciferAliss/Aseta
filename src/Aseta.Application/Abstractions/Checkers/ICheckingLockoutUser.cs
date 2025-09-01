using System.Security.Claims;

namespace Aseta.Application.Abstractions.Checkers;

public interface ICheckingLockoutUser
{
    Task<bool> CheckAsync(ClaimsPrincipal claims);
}
