using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Users.GetAll;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Users;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/all", async (
            IQueryHandler<GetAllUsersQuery, UsersResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetAllUsersQuery();

            Result<UsersResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(TagsApi.Users)
        .RequireAuthorization();
    }
}
