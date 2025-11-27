using Aseta.Domain.Abstractions.Primitives.Events;

namespace Aseta.Infrastructure.DomainEvents;

interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvent, CancellationToken cancellationToken = default);
}