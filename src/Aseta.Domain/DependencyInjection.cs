using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Services.CustomId;
using Microsoft.Extensions.DependencyInjection;

namespace Aseta.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ICustomIdService, CustomIdService>();
        
        return services;
    }
}
