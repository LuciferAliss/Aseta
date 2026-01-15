using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.Get.Contracts;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Categories;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Domain.Entities.Tags;

namespace Aseta.Application.Inventories.Create;

internal sealed class CreateInventoryCommandHandler(
    IInventoryRepository inventoryRepository,
    ICategoryRepository categoryRepository,
    ITagRepository tagRepository,
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

        IReadOnlyCollection<Tag> tags = [];
        if (command.TagIds.Count > 0)
        {
            tags = await tagRepository.FindAsync(t => command.TagIds.Contains(t.Id), cancellationToken: cancellationToken);
            if (tags.Count != command.TagIds.Count)
            {
                return TagErrors.NotFound();
            }
        }

        Result<Inventory> inventoryResult = Inventory.Create(
            command.Name,
            command.Description,
            command.ImageUrl!,
            command.IsPublic,
            command.CategoryId,
            tags.ToList(),
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
