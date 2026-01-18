using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;

namespace Aseta.Application.Tags.Delete;

internal sealed class DeleteTagsCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteTagsCommand>
{
    public async Task<Result> Handle(DeleteTagsCommand command, CancellationToken cancellationToken)
    {
        if (command.TagIds.Count == 0)
        {
            return Result.Success();
        }

        await using ITransactionScope transaction = await unitOfWork.BeginTransactionScopeAsync(cancellationToken);

        int deletedCount = await tagRepository.BulkRemoveAsync(
            tag => command.TagIds.Contains(tag.Id),
            cancellationToken);

        if (deletedCount != command.TagIds.Count)
        {
            return TagErrors.DeletionFailed();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
