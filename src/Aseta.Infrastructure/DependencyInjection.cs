using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Options;
using Aseta.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Aseta.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddRepositories()
            .AddAuthenticationInternal(configuration)
            .AddEmailSender(configuration)
            .ConfigureCookies()
            .AddDatabase(configuration)
            .AddRedis(configuration);
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        var dataSource = new NpgsqlDataSourceBuilder(connectionString)
            .EnableDynamicJson()
            .Build();

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(dataSource)
            .UseSnakeCaseNamingConvention());

        return services;
    } 

    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis");

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = "Aseta:";
        });

        return services;
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

            options.ExpireTimeSpan = TimeSpan.FromHours(24);
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.None;
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
            .AddFacebook(options =>
            {
                options.AppId = optionsFacebook.Id;
                options.AppSecret = optionsFacebook.Secret;
            });

        services.AddIdentityApiEndpoints<UserApplication>(opts =>
        {
            opts.SignIn.RequireConfirmedEmail = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }

    private static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthMessageSenderOptions>(configuration.GetSection(AuthMessageSenderOptions.SectionName));

        var authOptions = new AuthMessageSenderOptions();
        configuration.GetSection(AuthMessageSenderOptions.SectionName).Bind(authOptions);
        ArgumentException.ThrowIfNullOrWhiteSpace(authOptions.Key);

        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssembliesOf(
                typeof(DependencyInjection))
            .AddClasses(classes => classes.Where(type =>
                type.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());  
    } 
}