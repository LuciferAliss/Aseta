using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Categories.GetAll;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Categories;

internal sealed class GetGetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories/all", async (
            IQueryHandler<GetAllCategoriesQuery, CategoriesResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetAllCategoriesQuery();

            Result<CategoriesResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        }

        ).WithTags(Tags.Categories);
    }
}
