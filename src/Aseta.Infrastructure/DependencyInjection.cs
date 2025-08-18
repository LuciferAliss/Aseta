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
            .AddDatabase()
            .AddAuthenticationInternal()
            .AddEmailSender();

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        string? connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? throw new InvalidOperationException("Variable not found");

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

    private static IServiceCollection AddEmailSender(this IServiceCollection services)
    {
        services.Configure<AuthMessageSenderOptions>(options =>
        {
            options.SendGridKey = Environment.GetEnvironmentVariable(AuthMessageSenderOptions.SEND_GRID_KEY) ?? throw new InvalidOperationException("Variable not found");
        });

        services.AddTransient<IEmailSender, EmailSender>();
        
        return services;
    }
}
