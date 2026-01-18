using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Comments.GetPaginated;
using Aseta.Application.Comments.GetPaginated.Contracts;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.DTO.Comments;
using Microsoft.AspNetCore.Http;

namespace Aseta.API.Endpoints.Comments;

internal sealed class GetPaginated : IEndpoint
{
    public sealed record Request(
        Guid InventoryId,
        string? Cursor,
        int PageSize = 20,
        string SortBy = "Date",
        string SortOrder = "desc");

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/inventories/{inventoryId}/comments", async (
            [AsParameters] Request request,
            IQueryHandler<GetCommentsPaginatedQuery, CommentsResponse> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Enum.TryParse(request.SortBy, out SortBy sortBy);

            var query = new GetCommentsPaginatedQuery(
                request.InventoryId,
                sortBy,
                request.SortOrder,
                request.Cursor,
                request.PageSize
            );

            Result<CommentsResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(TagsApi.Comments);
    }
}
