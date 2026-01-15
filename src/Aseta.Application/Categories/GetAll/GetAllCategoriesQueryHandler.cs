using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Categories;

namespace Aseta.Application.Categories.GetAll;

internal sealed class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository) : IQueryHandler<GetAllCategoriesQuery, CategoriesResponse>
{
    public async Task<Result<CategoriesResponse>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Category> categories = await categoryRepository.GetAllAsync(cancellationToken: cancellationToken);

        var response = new CategoriesResponse(categories.Select(c => new CategoryResponse(c.Id, c.Name)).ToList());

        return response;
    }
}
