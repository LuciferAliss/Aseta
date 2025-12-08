using Aseta.API.Infrastructure;
using Aseta.Infrastructure.Options;

namespace Aseta.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ClientOptions>()
            .Bind(configuration.GetSection(ClientOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        ClientOptions optionsClient = new();
        configuration.GetSection(ClientOptions.SectionName).Bind(optionsClient);

        services.AddCors(opt => opt.AddPolicy("CorsPolicy", options => options.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(optionsClient.Url)));

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}