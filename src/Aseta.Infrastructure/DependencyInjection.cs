using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Options;
using Aseta.Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aseta.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDatabase(configuration)
            .AddAuthenticationInternal()
            .AddEmailSender(configuration);

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DATABASE_PRIVATE_UR");

        services.AddDbContext<AppDbContext>(
            options => options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .UseSnakeCaseNamingConvention());

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddAuthentication();

        services.AddIdentityApiEndpoints<UserApplication>(opts =>
        {
            opts.SignIn.RequireConfirmedEmail = true;
        }).AddEntityFrameworkStores<AppDbContext>();

        return services;
    }

    private static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthMessageSenderOptions>(configuration.GetSection(AuthMessageSenderOptions.SEND_GRID_KEY));
        services.AddTransient<IEmailSender, EmailSender>();
        
        return services;
    }
}
