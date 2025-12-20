using System.Security.Claims;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Http;

namespace Aseta.Infrastructure.Authentication;

internal sealed class UserContext(
    IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public UserRole? UserRole
    {
        get
        {
            string? roleString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

            return Enum.TryParse(roleString, out UserRole role) ? role : null;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public string? SessionId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Sid);
}
