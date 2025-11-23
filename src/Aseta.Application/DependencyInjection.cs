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
        services.AddHandlers()
            .AddDecorators()
            .AddAutoMapper(cfg => { }, typeof(DependencyInjection).Assembly)
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }

    private static IServiceCollection AddHandlers(
        this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableToAny(
                typeof(IQueryHandler<,>),
                typeof(ICommandHandler<>),
                typeof(ICommandHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
            
    private static IServiceCollection AddDecorators(
        this IServiceCollection services)
    {
        services.DecorateHandlers(
            typeof(ValidationDecorator.CommandHandler<,>),
            typeof(ValidationDecorator.CommandBaseHandler<>),
            typeof(ValidationDecorator.QueryHandler<,>));

        services.DecorateHandlers(
            typeof(AuthorizationDecorator.CommandHandler<,>),
            typeof(AuthorizationDecorator.CommandBaseHandler<>),
            typeof(AuthorizationDecorator.QueryHandler<,>));

        services.DecorateHandlers(
            typeof(LockedUserDecorator.CommandHandler<,>),
            typeof(LockedUserDecorator.CommandBaseHandler<>),
            typeof(LockedUserDecorator.QueryHandler<,>));

        services.DecorateHandlers(
            typeof(LoggingDecorator.CommandHandler<,>),
            typeof(LoggingDecorator.CommandBaseHandler<>),
            typeof(LoggingDecorator.QueryHandler<,>));

        return services;
    }

    private static IServiceCollection DecorateHandlers(
        this IServiceCollection services,
        Type commandHandlerDecorator,
        Type commandBaseHandlerDecorator,
        Type queryHandlerDecorator)
    {
        services.Decorate(typeof(ICommandHandler<,>), commandHandlerDecorator);
        services.Decorate(typeof(ICommandHandler<>), commandBaseHandlerDecorator);
        services.Decorate(typeof(IQueryHandler<,>), queryHandlerDecorator);
        return services;
    }
}