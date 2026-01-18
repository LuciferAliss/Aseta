using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Categories.Create;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Categories;

internal sealed class Create : IEndpoint
{
    public sealed record Request(string Name);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/categories", async (
            Request request,
            ICommandHandler<CreateCategoryCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            var command = new CreateCategoryCommand(request.Name);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(TagsApi.Categories)
        .RequireAuthorization();
    }
}
