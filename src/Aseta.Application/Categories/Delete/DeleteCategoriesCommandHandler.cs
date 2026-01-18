using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Categories;

namespace Aseta.Application.Categories.Delete;

internal sealed class DeleteCategoriesCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository) : ICommandHandler<DeleteCategoriesCommand>
{
    public async Task<Result> Handle(DeleteCategoriesCommand command, CancellationToken cancellationToken)
    {
        if (command.CategoryIds.Count == 0)
        {
            return Result.Success();
        }

        await using ITransactionScope transaction = await unitOfWork.BeginTransactionScopeAsync(cancellationToken);

        int deletedCount = await categoryRepository.BulkRemoveAsync(
            c => command.CategoryIds.Contains(c.Id),
            cancellationToken);

        if (deletedCount != command.CategoryIds.Count)
        {
            return CategoryErrors.DeletionFailed();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
