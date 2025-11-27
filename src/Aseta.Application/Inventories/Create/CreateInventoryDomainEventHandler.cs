using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.UserRoles;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Abstractions.Primitives.Events;

namespace Aseta.Application.Inventories.Create;

internal sealed class CreateInventoryDomainEventHandler(
    IInventoryUserRoleRepository inventoryUserRoleRepository) : IDomainEventHandler<CreateInventoryDomainEvent>
{
    public async Task<Result> Handle(CreateInventoryDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var role = InventoryRole.Create(domainEvent.UserId, domainEvent.InventoryId, Role.Owner);

        await inventoryUserRoleRepository.AddAsync(role, cancellationToken);

        return Result.Success();
    }
}
