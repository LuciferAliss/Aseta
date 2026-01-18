using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Comments;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Application.Comments.Add;

internal sealed class AddCommentCommandHandler(
    IInventoryRepository inventoryRepository,
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddCommentCommand>
{
    public async Task<Result> Handle(AddCommentCommand command, CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken, i => i.Comments);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(command.InventoryId);
        }

        Result<Comment> commentResult = Comment.Create(command.Content, command.UserId, command.InventoryId);

        if (commentResult.IsFailure)
        {
            return commentResult.Error;
        }

        Comment comment = commentResult.Value;

        await commentRepository.AddAsync(comment, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
