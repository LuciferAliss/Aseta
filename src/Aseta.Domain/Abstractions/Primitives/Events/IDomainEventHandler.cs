using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.Domain.Abstractions.Primitives.Events;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task<Result> Handle(T domainEvent, CancellationToken cancellationToken);
}