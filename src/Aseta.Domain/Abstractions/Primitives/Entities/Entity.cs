using Aseta.Domain.Abstractions.Primitives.Events;

namespace Aseta.Domain.Abstractions.Primitives.Entities;

public abstract class Entity : IEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public Guid Id { get; init; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity() {}

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}