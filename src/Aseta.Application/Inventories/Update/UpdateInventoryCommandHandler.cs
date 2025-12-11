using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Categories;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.Inventories.Update;

internal sealed class UpdateInventoryCommandHandler(
    IInventoryRepository inventoryRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateInventoryCommand>
{
    public async Task<Result> Handle(
        UpdateInventoryCommand command,
        CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken: cancellationToken);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        bool categoryExists = await categoryRepository.ExistsAsync(c => c.Id == command.CategoryId, cancellationToken: cancellationToken);

        if (!categoryExists)
        {
            return CategoryErrors.NotFound(command.CategoryId);
        }

        Result updateResult = inventory.Update(
            command.Name,
            command.Description,
            command.ImageUrl!,
            command.CategoryId,
            command.IsPublic);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
