using System.Text;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Services;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Authentication;
using Aseta.Infrastructure.Authorization;
using Aseta.Infrastructure.Caches;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.DomainEvents;
using Aseta.Infrastructure.Options;
using Aseta.Infrastructure.Persistence.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace Aseta.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDatabase(configuration)
            .AddRedis(configuration)
            .AddAuthentication(configuration)
            .AddAppAuthorization()
            .AddDomainEventsDispatcher()
            .AddHealthChecks(configuration);
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        NpgsqlDataSource dataSource = new NpgsqlDataSourceBuilder(connectionString)
            .EnableDynamicJson()
            .Build();

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(dataSource)
            .UseSnakeCaseNamingConvention());

        services.Scan(scan => scan.FromAssembliesOf(
                typeof(DependencyInjection))
            .AddClasses(classes => classes.Where(type =>
                type.Name.EndsWith("Repository", StringComparison.Ordinal)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Redis");

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = "Aseta:";
        });

        services.AddScoped<ICacheService, CacheService>();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<RefreshTokenOptions>()
            .Bind(configuration.GetSection(RefreshTokenOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        JwtOptions jwtOptions = new();
        configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);

        RefreshTokenOptions refreshTokenOptions = new();
        configuration.GetSection(RefreshTokenOptions.SectionName).Bind(refreshTokenOptions);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false; // ! In production, this should be true.
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    private static IServiceCollection AddDomainEventsDispatcher(this IServiceCollection services)
    {
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();

        return services;
    }

    private static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddAuthorization();
        services.AddScoped<ILockedUserChecker, LockedUserChecker>();
        services.AddScoped<IUserSessionChecker, UserSessionChecker>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IUserRoleChecker, UserRoleChecker>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!)
            .AddRedis(configuration.GetConnectionString("Redis")!);

        return services;
    }
}