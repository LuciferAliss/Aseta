using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Categories.Delete;
using Aseta.Domain.Abstractions.Primitives.Results;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Endpoints.Categories;

internal sealed class Delete : IEndpoint
{
    public sealed record Request(string[] CategoryIds);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/categories", async (
            [FromBody] Request request,
            ICommandHandler<DeleteCategoriesCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            ICollection<Guid> categoryIds = request.CategoryIds.Select(x =>
            {
                _ = Guid.TryParse(x, out Guid id);
                return id;
            }).ToList();

            var command = new DeleteCategoriesCommand(categoryIds);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        }
        )
        .WithTags(TagsApi.Categories)
        .RequireAuthorization();
    }
}

