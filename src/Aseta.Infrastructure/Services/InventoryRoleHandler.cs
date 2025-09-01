using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Aseta.Domain.Entities.Users;
using System.Security.Claims;
using Aseta.Infrastructure.Requirements;
using Microsoft.Extensions.DependencyInjection;
using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Infrastructure.Services;

public class InventoryRoleHandler(IServiceScopeFactory scopeFactory) : AuthorizationHandler<InventoryRoleRequirement, Guid>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, InventoryRoleRequirement requirement, Guid inventoryId)
    {
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            context.Fail();
            return;
        }

        await using var scope = scopeFactory.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserApplication>>();

        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            context.Fail();
            return;
        }

        if (await userManager.IsInRoleAsync(user, "Admin"))
        {
            context.Succeed(requirement);
            return;
        }

        var inventoryUserRoleRepository = scope.ServiceProvider.GetRequiredService<IInventoryUserRoleRepository>();

        if (await inventoryUserRoleRepository.UserHasRoleAsync(user.Id, inventoryId, requirement.RequiredRole))
        {
            context.Succeed(requirement);
            return;
        }
        
        context.Fail();
    }
}