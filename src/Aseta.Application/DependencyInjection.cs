using Aseta.Application.Abstractions.Behaviors;
using Aseta.Application.Abstractions.Messaging;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Aseta.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddHandlerFromAssembly()
            .AddDecorateFromAssembly()
            .AddAutoMapper(
                cfg => { },
                typeof(DependencyInjection).Assembly)
            .AddValidatorsFromAssembly(
                typeof(DependencyInjection).Assembly,
                includeInternalTypes: true);

        return services;
    }

    private static IServiceCollection AddHandlerFromAssembly(
        this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }
            
    private static IServiceCollection AddDecorateFromAssembly(
        this IServiceCollection services)
    {
        services.Decorate(
            typeof(ICommandHandler<,>),
            typeof(ValidationDecorator.CommandHandler<,>));
        services.Decorate(
            typeof(ICommandHandler<>),
            typeof(ValidationDecorator.CommandBaseHandler<>));
        services.Decorate(
            typeof(IQueryHandler<,>),
            typeof(ValidationDecorator.QueryHandler<,>));

        services.Decorate(
            typeof(ICommandHandler<,>), 
            typeof(LoggingDecorator.CommandHandler<,>));
        services.Decorate(
            typeof(ICommandHandler<>),
            typeof(LoggingDecorator.CommandBaseHandler<>));
        services.Decorate(
            typeof(IQueryHandler<,>),
            typeof(LoggingDecorator.QueryHandler<,>));

        services.Decorate(
            typeof(ICommandHandler<,>),
            typeof(AuthorizationDecorator.CommandHandler<,>));
        services.Decorate(
            typeof(ICommandHandler<>),
            typeof(AuthorizationDecorator.CommandBaseHandler<>));
        services.Decorate(
            typeof(IQueryHandler<,>),
            typeof(AuthorizationDecorator.QueryHandler<,>));

        return services;
    }
}