using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Events;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.UserRoles;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Domain.Entities.Users;

public class ApplicationUser : IdentityUser<Guid>, IEntity
{
    public new string UserName { get; set; }
    public virtual ICollection<Inventory> Inventories { get; private set; } = [];
    public virtual ICollection<InventoryRole> InventoryUserRoles { get; private set; } = [];
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}