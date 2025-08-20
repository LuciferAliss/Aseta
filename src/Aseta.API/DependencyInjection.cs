using Aseta.API.Extensions;
using Aseta.Infrastructure.Options;

namespace Aseta.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        ClientOptions optionsClient = new();
        configuration.GetSection(ClientOptions.SectionName).Bind(optionsClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(optionsClient.Url);

        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", options =>
            {               
                options.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(optionsClient.Url);
            });
        });

        services.AddControllers();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}