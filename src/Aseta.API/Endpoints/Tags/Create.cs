using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Tags.Create;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Tags;

internal sealed class Create : IEndpoint
{
    public sealed record Request(string Name);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/tags", async (
            Request request,
            ICommandHandler<CreateTagCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            var command = new CreateTagCommand(request.Name);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(TagsApi.Tags)
        .RequireAuthorization();
    }
}
