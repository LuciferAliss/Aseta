using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Comments.GetPaginated.Contracts;
using Aseta.Domain.DTO.Comments;

namespace Aseta.Application.Comments.GetPaginated;

public sealed record GetCommentsPaginatedQuery(
    Guid InventoryId,
    SortBy SortBy,
    string SortOrder,
    string? Cursor,
    int PageSize) : IQuery<CommentsResponse>;