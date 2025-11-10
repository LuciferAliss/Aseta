using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Enums;

namespace Aseta.Application.Inventories.Create;

internal sealed class CreateInventoryCommandHandler(
    IInventoryRepository inventoryRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateInventoryCommand, InventoryResponse>
{
    public async Task<Result<InventoryResponse>> Handle(
        CreateInventoryCommand command,
        CancellationToken cancellationToken)
    {
        var inventoryResult = Inventory.Create(
            command.Name,
            command.Description,
            command.ImageUrl,
            command.IsPublic,
            command.CategoryId,
            command.UserId);

        if (inventoryResult.IsFailure) return inventoryResult.Error;

        await inventoryRepository.AddAsync(inventoryResult.Value, cancellationToken);

        var inventoryUserRoleResult = InventoryRole.Create(command.UserId, inventoryResult.Value.Id, Role.Owner);

        if (inventoryUserRoleResult.IsFailure) return inventoryUserRoleResult.Error;

        await inventoryUserRoleRepository.AddAsync(inventoryUserRoleResult.Value, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new InventoryResponse(inventoryResult.Value.Id);
    }
}
