using Aseta.Application.Abstractions.Checkers;
using Aseta.Application.Abstractions.Services;
using Aseta.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aseta.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IInventoryService, InventoryService>();

        return services;
    }
}
