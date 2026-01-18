using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Comments;

namespace Aseta.Application.Comments.Delete;

internal sealed class DeleteCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteCommentCommand>
{
    public async Task<Result> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
    {
        Comment? comment = await commentRepository.GetByIdAsync(command.CommentId, true, cancellationToken);

        if (comment is null)
        {
            return CommentErrors.NotFound(command.CommentId);
        }

        if (comment.UserId != command.UserId && !command.IsAdmin)
        {
            return CommentErrors.NotOwner(command.CommentId, command.UserId);
        }

        commentRepository.Remove(comment);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
