using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Items;

namespace Aseta.Application.Items.Delete;

internal sealed class DeleteItemsCommandHandler(
    IItemRepository IItemRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteItemsCommand>
{
    public async Task<Result> Handle(
        DeleteItemsCommand command,
        CancellationToken cancellationToken)
    {
        if (command.ItemIds.Count == 0) return Result.Success();

        await using var transaction = await unitOfWork.BeginTransactionScopeAsync(cancellationToken);
        
        var deletedCount = await IItemRepository.RemoveAsync(
            i => command.ItemIds.Contains(i.Id),
            cancellationToken);
        if (deletedCount != command.ItemIds.Count) return ItemErrors.DeletionFailed();

        await transaction.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
