using System.Collections.Concurrent;
using Aseta.Domain.Abstractions.Primitives.Events;
using Aseta.Domain.Abstractions.Primitives.Results;
using Microsoft.Extensions.DependencyInjection;

namespace Aseta.Infrastructure.DomainEvents;

internal sealed class DomainEventsDispatcher(IServiceScopeFactory serviceProvider) : IDomainEventsDispatcher
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public async Task<Result> DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        var handlerResults = new List<Result>();
        
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            Type domainEventType = domainEvent.GetType();
            Type handlerType = HandlerTypeDictionary.GetOrAdd(
                domainEventType,
                et => typeof(IDomainEventHandler<>).MakeGenericType(et));

            IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (object? handler in handlers)
            {
                if (handler is null)
                {
                    continue;
                }

                var handlerWrapper = HandlerWrapper.Create(handler, domainEventType);

                handlerResults.Add(await handlerWrapper.Handle(domainEvent, cancellationToken));
            }
        }

        if (handlerResults.Any(r => r.IsFailure))
        {
            return DomainEventsDispatcherErrors.HandlersFailed;
        }

        return Result.Success();
    }

    private abstract class HandlerWrapper
    {
        public abstract Task<Result> Handle(IDomainEvent domainEvent, CancellationToken cancellationToken);

        public static HandlerWrapper Create(object handler, Type domainEventType)
        {
            Type wrapperType = WrapperTypeDictionary.GetOrAdd(
                domainEventType,
                et => typeof(HandlerWrapper<>).MakeGenericType(et));

            return (HandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
        }
    }

    private sealed class HandlerWrapper<T>(object handler) : HandlerWrapper where T : IDomainEvent
    {
        private readonly IDomainEventHandler<T> _handler = (IDomainEventHandler<T>)handler;

        public override async Task<Result> Handle(IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return await _handler.Handle((T)domainEvent, cancellationToken);
        }
    }
}
