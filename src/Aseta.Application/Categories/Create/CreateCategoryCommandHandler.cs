using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Categories;

namespace Aseta.Application.Categories.Create;

internal sealed class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateCategoryCommand>
{
    public async Task<Result> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetByNameAsync(command.Name, cancellationToken: cancellationToken);

        if (category is not null)
        {
            return CategoryErrors.AlreadyExists(command.Name);
        }

        Result<Category> categoryResult = Category.Create(command.Name);

        if (categoryResult.IsFailure)
        {
            return categoryResult.Error;
        }

        category = categoryResult.Value;

        await categoryRepository.AddAsync(category, cancellationToken: cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
