using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Categories.Update;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Categories;

internal sealed class Update : IEndpoint
{
    public sealed record Request(string Name);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/categories/{id}", async (
            string id,
            Request request,
            ICommandHandler<UpdateCategoryCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid categoryId);

            var command = new UpdateCategoryCommand(categoryId, request.Name);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.Categories)
        .RequireAuthorization();
    }
}
