using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.InventoryRoles.Add;

internal sealed class AddRoleUserCommandHandler(
    IInventoryRepository inventoryRepository,
    IUserRepository userRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddRoleUserCommand>
{
    public async Task<Result> Handle(AddRoleUserCommand command, CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken: cancellationToken);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        User? user = await userRepository.GetByIdAsync(command.UserId, cancellationToken: cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound(command.UserId.ToString());
        }

        InventoryRole? role = await inventoryUserRoleRepository.GetUserRoleInInventory(user.Id, command.InventoryId, cancellationToken);

        if (role?.Role.ToString() == command.Role.ToString())
        {
            return InventoryRoleErrors.UserAlreadyRole(user.Id, command.InventoryId, command.Role.ToString());
        }

        Result<InventoryRole> inventoryRoleResult = InventoryRole.Create(user.Id, command.InventoryId, command.Role);

        if (inventoryRoleResult.IsFailure)
        {
            return inventoryRoleResult.Error;
        }

        InventoryRole inventoryRole = inventoryRoleResult.Value;

        await inventoryUserRoleRepository.AddAsync(inventoryRole, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
