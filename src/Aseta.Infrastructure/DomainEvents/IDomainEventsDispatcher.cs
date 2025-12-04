using Aseta.Domain.Abstractions.Primitives.Events;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.Infrastructure.DomainEvents;

public interface IDomainEventsDispatcher
{
    Task<Result> DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}