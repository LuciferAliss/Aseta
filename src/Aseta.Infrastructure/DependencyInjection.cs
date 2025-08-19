using System.Runtime.CompilerServices;
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
    private const string CONNECTION_STRING_FOR_DATABASE = "DATABASE_URL";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDev) =>
        services
            .AddDatabase(configuration, isDev)
            .AddAuthenticationInternal()
            .AddEmailSender(configuration, isDev);

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, bool isDev)
    {
        string connectionString; 
        if (isDev) 
            connectionString = configuration.GetConnectionString(CONNECTION_STRING_FOR_DATABASE) ?? throw new InvalidOperationException("Variable `DATABASE_URL` not found in local development"); 
        else
        { 
            var connUrl = Environment.GetEnvironmentVariable(CONNECTION_STRING_FOR_DATABASE) ?? throw new InvalidOperationException("Variable `DATABASE_URL` not found in production"); 

            var databaseUri = new Uri(connUrl); 
            var userInfo = databaseUri.UserInfo.Split(':'); 
            var Host = databaseUri.Host; 
            var Port = databaseUri.Port; 
            var UserName = userInfo[0]; 
            var Password = userInfo[1]; 
            var Database = databaseUri.LocalPath.TrimStart('/'); 
            
            connectionString = $"Server={Host};Port={Port};UserId={UserName} ;Password={Password};Database={Database};"; 
        }

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

    private static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration, bool isDev)
    {
        string connectionString;
        if (isDev)
            connectionString = configuration.GetConnectionString(AuthMessageSenderOptions.SEND_GRID_KEY) ?? throw new InvalidOperationException("Variable `SEND_GRID_KEY` not found in local development");
        else
        {
            connectionString = Environment.GetEnvironmentVariable(AuthMessageSenderOptions.SEND_GRID_KEY) ?? throw new InvalidOperationException("Variable not found in production");
        }

        services.Configure<AuthMessageSenderOptions>(options =>
        {
            options.SendGridKey = connectionString;
        });

        services.AddTransient<IEmailSender, EmailSender>();
        
        return services;
    }
}
