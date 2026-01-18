using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Comments.Delete;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.API.Endpoints.Comments;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/comments/{commentId}", async (
            string commentId,
            ICommandHandler<DeleteCommentCommand> handler,
            IUserContext user,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(user.UserId, out Guid userId);
            _ = Guid.TryParse(commentId, out Guid commentIdGuid);

            var command = new DeleteCommentCommand(commentIdGuid, userId, user.UserRole == UserRole.Admin);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.Comments)
        .RequireAuthorization();
    }
}
