using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.InventoryRoles.Update;

internal sealed class UpdateRoleUserCommandHandler(
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateRoleUserCommand>
{
    public async Task<Result> Handle(UpdateRoleUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(command.UserId, cancellationToken: cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound(command.UserId.ToString());
        }

        InventoryRole? role = await inventoryUserRoleRepository.GetUserRoleInInventory(user.Id, command.InventoryId, cancellationToken: cancellationToken);

        if (role is null)
        {
            return InventoryRoleErrors.UserHasNoRole(user.Id, command.InventoryId);
        }

        if (role.Role == command.Role)
        {
            return InventoryRoleErrors.UserAlreadyRole(user.Id, command.InventoryId, command.Role.ToString());
        }

        role.UpdateRole(command.Role);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
