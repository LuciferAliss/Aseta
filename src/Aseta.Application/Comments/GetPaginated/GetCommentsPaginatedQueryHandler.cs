using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Comments.GetPaginated.Contracts;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Pagination;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.DTO.Comments;
using Aseta.Domain.Entities.Comments;

namespace Aseta.Application.Comments.GetPaginated;

internal sealed class GetCommentsPaginatedQueryHandler(
    ICommentRepository commentRepository) : IQueryHandler<GetCommentsPaginatedQuery, CommentsResponse>
{
    public async Task<Result<CommentsResponse>> Handle(
        GetCommentsPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        var paginationParameters = new CommentPaginationParameters(
            query.SortBy,
            query.SortOrder,
            query.Cursor,
            query.PageSize
        );

        (ICollection<Comment>? comments, string? nextCursor, bool hasNextPage) = await commentRepository
            .GetPaginatedWithKeysetAsync(paginationParameters, query.InventoryId, cancellationToken);

        ICollection<CommentResponse> commentResponses = comments.Select(c => new CommentResponse(
            c.Id,
            c.Content,
            c.User.UserName,
            c.User.Email,
            c.CreatedAt)).ToList();

        var paginationResult = new KeysetPage<CommentResponse>(nextCursor, hasNextPage, commentResponses);

        return new CommentsResponse(paginationResult);
    }
}