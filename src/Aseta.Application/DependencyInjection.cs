using Aseta.Application.Abstractions.Behaviors;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Primitives.Events;
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
                typeof(ICommandHandler<,>),
                typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddDecorators(
        this IServiceCollection services)
    {
        services.Decorate(typeof(ICommandHandler<,>), typeof(AuthorizationDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(AuthorizationDecorator.CommandBaseHandler<>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(AuthorizationDecorator.QueryHandler<,>));

        services.Decorate(typeof(ICommandHandler<,>), typeof(LockedUserDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(LockedUserDecorator.CommandBaseHandler<>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(LockedUserDecorator.QueryHandler<,>));

        services.Decorate(typeof(ICommandHandler<,>), typeof(SessionDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(SessionDecorator.CommandBaseHandler<>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(SessionDecorator.QueryHandler<,>));

        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandBaseHandler<>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(ValidationDecorator.QueryHandler<,>));

        services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.CommandBaseHandler<>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));

        // services.Decorate(typeof(IDomainEventHandler<>), typeof(LoggingDecorator.DomainEventHandler<>));

        return services;
    }
}