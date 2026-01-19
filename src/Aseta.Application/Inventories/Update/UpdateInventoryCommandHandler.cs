using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Categories;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Tags;

namespace Aseta.Application.Inventories.Update;

internal sealed class UpdateInventoryCommandHandler(
    IInventoryRepository inventoryRepository,
    ICategoryRepository categoryRepository,
    ITagRepository tagRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateInventoryCommand>
{
    public async Task<Result> Handle(
        UpdateInventoryCommand command,
        CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken: cancellationToken, i => i.Tags);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

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

        Result updateResult = inventory.Update(
            command.Name,
            command.Description,
            command.ImageUrl!,
            command.CategoryId,
            tags.ToList(),
            command.IsPublic);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
