using Aseta.Domain.Abstractions.Primitives.Events;

namespace Aseta.Domain.Abstractions.Primitives.Entities;

public interface IEntity
{
    Guid Id { get; }
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}