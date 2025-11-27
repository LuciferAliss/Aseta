using Aseta.Domain.Abstractions.Primitives.Events;

namespace Aseta.Domain.Entities.Inventories;

public sealed record CreateInventoryDomainEvent(Guid InventoryId, Guid UserId) : IDomainEvent;