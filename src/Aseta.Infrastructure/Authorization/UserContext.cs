using System.Security.Claims;
using Aseta.Application.Abstractions.Authorization;
using Microsoft.AspNetCore.Http;

namespace Aseta.Infrastructure.Authorization;

internal sealed class UserContext(
    IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
