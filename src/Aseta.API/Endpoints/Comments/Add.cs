using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Comments.Add;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Comments;

internal sealed class Add : IEndpoint
{
    public sealed record Request(string Content);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/{id}/comments", async (
            string id,
            IUserContext user,
            Request request,
            ICommandHandler<AddCommentCommand> handler,
            CancellationToken cancellationToken = default
        ) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);
            _ = Guid.TryParse(user.UserId, out Guid userId);

            var command = new AddCommentCommand(request.Content, inventoryId, userId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(TagsApi.Comments)
        .RequireAuthorization();
    }
}
