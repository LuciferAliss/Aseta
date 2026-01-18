using Aseta.Domain.Abstractions.Primitives.Pagination;

namespace Aseta.Application.Comments.GetPaginated.Contracts;

public sealed record CommentsResponse(
    KeysetPage<CommentResponse> Comments
);