using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Options;
using Aseta.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
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
        .AddAuthenticationInternal(configuration)
        .AddEmailSender(configuration)
        .ConfigureCookies()
        .AddDatabase(configuration);

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseOptions options = new();
        configuration.GetSection(DatabaseOptions.SectionName).Bind(options);
        
        string connectionString = ConfigurationConnectionStringDb(options.Url);

        services.AddDbContext<AppDbContext>(
            options => options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .UseSnakeCaseNamingConvention());

        return services;
    }

    private static string ConfigurationConnectionStringDb(string? connUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connUrl);

        if (!Uri.TryCreate(connUrl, UriKind.Absolute, out var databaseUri))
        {
            return connUrl;
        }
        
        var userInfo = databaseUri.UserInfo.Split(':');
        var Host = databaseUri.Host;
        var Port = databaseUri.Port;
        var UserName = userInfo[0];
        var Password = userInfo[1];
        var Database = databaseUri.LocalPath.TrimStart('/');

        return $"Server={Host};Port={Port};UserId={UserName};Password={Password};Database={Database};";
    }

    private static IServiceCollection ConfigureCookies(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };

            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.HttpOnly = true;
            // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; for production
        });

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services, IConfiguration configuration)
    {
        var optionsFacebook = new FacebookOption();
        configuration.GetSection(FacebookOption.SectionName).Bind(optionsFacebook);

        if (string.IsNullOrWhiteSpace(optionsFacebook.Id) || string.IsNullOrWhiteSpace(optionsFacebook.Secret))
        {
            throw new ArgumentException("Null Facebook Id or Secret");
        }

        services.AddAuthentication()
            .AddCookie(options =>
            {
                options.SlidingExpiration = true;
            })
            .AddFacebook(options =>
            {
                options.AppId = optionsFacebook.Id;
                options.AppSecret = optionsFacebook.Secret;
            });

        services.AddIdentityApiEndpoints<UserApplication>(opts =>
        {
            opts.SignIn.RequireConfirmedEmail = true;
        }).AddEntityFrameworkStores<AppDbContext>();

        return services;
    }

    private static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        AuthMessageSenderOptions options = new();
        configuration.GetSection(AuthMessageSenderOptions.SectionName).Bind(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Key);

        services.Configure<AuthMessageSenderOptions>(options =>
        {
            options.Key = options.Key;
        });

        services.AddTransient<IEmailSender, EmailSender>();
        
        return services;
    }
}
