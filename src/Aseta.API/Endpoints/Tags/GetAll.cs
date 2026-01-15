using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Tags.GetAll;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Tags;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/tags/all", async (
            IQueryHandler<GetAllTagsQuery, TagsResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAllTagsQuery();

            Result<TagsResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        }).WithTags(TagsApi.Tags);
    }
}
