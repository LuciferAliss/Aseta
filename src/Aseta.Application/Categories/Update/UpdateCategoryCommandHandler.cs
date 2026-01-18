using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Categories;

namespace Aseta.Application.Categories.Update;

internal sealed class UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetByIdAsync(command.CategoryId, true, cancellationToken: cancellationToken);

        if (category is null)
        {
            return CategoryErrors.NotFound(command.CategoryId);
        }

        Result updateResult = category.Update(command.Name);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
