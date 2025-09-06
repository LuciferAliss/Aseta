using System.Threading.Tasks;
using Aseta.Application.Abstractions.Checkers;
using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Checkers;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Options;
using Aseta.Infrastructure.Repository;
using Aseta.Infrastructure.Requirements;
using Aseta.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Aseta.Infrastructure;

public static class DependencyInjection
{
    public static async Task<IServiceCollection> AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return await services
            .AddRepositories()
            .AddAuthenticationInternal(configuration)
            .AddEmailSender(configuration)
            .ConfigureCookies()
            .AddDatabase(configuration)
            .AddPolicies()
            .AddCheckers()
            .AddRoleAdmin();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseOptions options = new();
        configuration.GetSection(DatabaseOptions.SectionName).Bind(options);

        string connectionString = ConfigurationConnectionStringDb(options.Url);

        var dataSource = new NpgsqlDataSourceBuilder(connectionString)
            .EnableDynamicJson()
            .Build();

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(dataSource, npgsqlOptions =>
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

    private static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanManageInventory", policy =>
                policy.Requirements.Add(new InventoryRoleRequirement(InventoryRole.Owner)));
            options.AddPolicy("CanEditInventory", policy =>
                policy.Requirements.Add(new InventoryRoleRequirement(InventoryRole.Editor)));
        });
        
        services.AddSingleton<IAuthorizationHandler, InventoryRoleHandler>();
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

    private static async Task<IServiceCollection> AddRoleAdmin(this IServiceCollection services)
    {
        var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<UserApplication>>();
        var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        await RoleInitializer.InitializeAsync(userManager, roleManager);
        
        return services;
    }

    private static IServiceCollection AddCheckers(this IServiceCollection services)
    {
        services.AddScoped<ICheckingAccessPolicy, CheckingAccessPolicy>();
        services.AddScoped<ICheckingLockoutUser, CheckingLockoutUser>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IInventoryUserRoleRepository, InventoryUserRoleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}