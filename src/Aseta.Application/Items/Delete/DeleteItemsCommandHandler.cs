using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Application.Items.Delete;

internal sealed class DeleteItemsCommandHandler(
    IItemRepository IItemRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteItemsCommand>
{
    public async Task<Result> Handle(DeleteItemsCommand command, CancellationToken cancellationToken)
    {
        await IItemRepository.DeleteByItemIdsAsync(command.ItemIds, command.InventoryId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
