using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.Get.Contracts;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Categories;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Application.Inventories.Create;

internal sealed class CreateInventoryCommandHandler(
    IInventoryRepository inventoryRepository,
    ICategoryRepository categoryRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateInventoryCommand, InventoryResponse>
{
    public async Task<Result<InventoryResponse>> Handle(
        CreateInventoryCommand command,
        CancellationToken cancellationToken)
    {
        bool categoryExists = await categoryRepository.ExistsAsync(c => c.Id == command.CategoryId, cancellationToken: cancellationToken);

        if (!categoryExists)
        {
            return CategoryErrors.NotFound(command.CategoryId);
        }

        Result<Inventory> inventoryResult = Inventory.Create(
            command.Name,
            command.Description,
            command.ImageUrl!,
            command.IsPublic,
            command.CategoryId,
            command.UserId);

        if (inventoryResult.IsFailure)
        {
            return inventoryResult.Error;
        }

        Inventory inventory = inventoryResult.Value;

        await inventoryRepository.AddAsync(inventory, cancellationToken);

        Result<InventoryRole> roleResult = InventoryRole.Create(inventory.CreatorId, inventory.Id, Role.Owner);

        if (roleResult.IsFailure)
        {
            return roleResult.Error;
        }

        InventoryRole role = roleResult.Value;

        await inventoryUserRoleRepository.AddAsync(role, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new InventoryResponse(inventory.Id);
    }
}
