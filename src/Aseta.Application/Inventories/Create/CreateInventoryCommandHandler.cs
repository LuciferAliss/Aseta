using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.UserRoles;

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
            command.UserId,
            DateTime.UtcNow);

        await inventoryRepository.AddAsync(inventory, cancellationToken);

        var role = InventoryRole.Create(inventory.CreatorId, inventory.Id, Role.Owner);
        await inventoryUserRoleRepository.AddAsync(role, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new InventoryResponse(inventory.Id);
    }
}
