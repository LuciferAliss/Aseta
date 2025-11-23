using Aseta.Domain.Abstractions.Primitives;

namespace Aseta.Infrastructure.DomainEvents;

 interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}