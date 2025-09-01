using System.Security.Claims;
using Aseta.Application.Abstractions.Checkers;
using Microsoft.AspNetCore.Authorization;

namespace Aseta.Infrastructure.Checkers;

public class CheckingAccessPolicy(IAuthorizationService authorizationService) : ICheckingAccessPolicy
{
    private readonly IAuthorizationService _authorizationService1 = authorizationService;

    public async Task<bool> CheckAsync(ClaimsPrincipal claims, Guid inventoryId, string policyName)
    {
        var authorizationResult = await _authorizationService1.AuthorizeAsync(claims, inventoryId, policyName);
        return authorizationResult.Succeeded;
    }
}
