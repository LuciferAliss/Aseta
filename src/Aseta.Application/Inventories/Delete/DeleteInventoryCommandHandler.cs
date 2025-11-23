using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.Inventories.Delete;

internal sealed class DeleteInventoryCommandHandler(
    IInventoryRepository inventoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteInventoryCommand>
{
    public async Task<Result> Handle(
        DeleteInventoryCommand command,
        CancellationToken cancellationToken)
    {
        await using var transaction = await unitOfWork.BeginTransactionScopeAsync(cancellationToken);
        
        int deletedCount = await inventoryRepository.RemoveAsync(i =>
            i.Id == command.InventoryId, cancellationToken);
        if (deletedCount == 0) return InventoryErrors.NotFound(command.InventoryId);

        await transaction.CommitAsync(cancellationToken);
        return Result.Success();
    }
}