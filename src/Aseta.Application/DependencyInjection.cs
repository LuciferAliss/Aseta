using Aseta.Application.Abstractions.Services;
using Aseta.Application.Mapping;
using Aseta.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aseta.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IInventoryPermissionService, InventoryPermissionService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IAuthService, AuthService>();
        
        services.AddAutoMapper(opt =>
            opt.AddProfile<MappingProfile>()
        );

        return services;
    }
}
