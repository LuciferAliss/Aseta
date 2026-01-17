using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Users.SearchUser;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Users;

internal sealed class SearchUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/search/{searchTerm}", async (
            string searchTerm,
            IQueryHandler<SearchUserQuery, UsersResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            var query = new SearchUserQuery(searchTerm);

            Result<UsersResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(TagsApi.Users)
        .RequireAuthorization();
    }
}
