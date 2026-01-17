using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Users.GetByEmail;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Users;

internal sealed class GetByEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/email/{email}", async (
            string email,
            IQueryHandler<GetByEmailQuery, UserResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetByEmailQuery(email);

            Result<UserResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(TagsApi.Users)
        .RequireAuthorization();
    }
}
