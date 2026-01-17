using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.InventoryRoles.Delete;

public class DeleteRoleUserCommandHandler(
    IInventoryRepository inventoryRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteRoleUserCommand>
{
    public async Task<Result> Handle(DeleteRoleUserCommand command, CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken: cancellationToken, i => i.UserRoles);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        InventoryRole? inventoryRole = inventory.UserRoles.FirstOrDefault(x => x.UserId == command.DeletedUserId && x.InventoryId == command.InventoryId);

        if (inventoryRole is null)
        {
            return InventoryRoleErrors.UserHasNoRole(command.DeletedUserId, command.InventoryId);
        }

        inventoryUserRoleRepository.Remove(inventoryRole);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
