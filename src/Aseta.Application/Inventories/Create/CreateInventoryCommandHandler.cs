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
        var inventory = Inventory.Create(
            command.Name,
            command.Description,
            command.ImageUrl,
            command.IsPublic,
            command.CategoryId,
            command.UserId);

        await inventoryRepository.AddAsync(inventory, cancellationToken);

        await inventoryUserRoleRepository.AddAsync(
            InventoryUserRole.Create(
                command.UserId,
                inventory.Id,
                Role.Owner), cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new InventoryResponse(inventory.Id);
    }
}
