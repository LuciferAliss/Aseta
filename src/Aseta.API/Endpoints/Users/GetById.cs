using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Users.GetById;
using Aseta.Domain.Abstractions.Primitives.Results;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Endpoints.Users;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (
            IUserContext user,
            IQueryHandler<GetByIdQuery, UserResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(user.UserId, out Guid userIdGuid);

            var query = new GetByIdQuery(userIdGuid);

            Result<UserResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(TagsApi.Users)
        .RequireAuthorization();
    }
}
