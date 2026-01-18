using Aseta.Domain.Abstractions.Primitives.Pagination;

namespace Aseta.Domain.DTO.Comments;

public sealed record CommentPaginationParameters(
    SortBy SortBy,
    string SortOrder,
    string? Cursor,
    int PageSize);