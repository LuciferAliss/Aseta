using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.DTO.Comments;
using Aseta.Domain.Entities.Comments;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Pagination;
using Aseta.Infrastructure.Persistence.Common;

namespace Aseta.Infrastructure.Persistence.Comments;

public sealed class CommentRepository(AppDbContext context) : Repository<Comment>(context), ICommentRepository
{
    public async Task<(ICollection<Comment> comments, string? nextCursor, bool hasNextPage)> GetPaginatedWithKeysetAsync(
        CommentPaginationParameters parameters,
        Guid inventoryId,
        CancellationToken cancellationToken)
    {
        IQueryable<Comment> query = _dbSet.Where(c => c.InventoryId == inventoryId)
            .ApplyInclude(c => c.User)
            .ApplyTracking(false);

        (ICollection<Comment>? items, string? nextCursor, bool hasNextPage) = await new KeysetPaginator<Comment>(query)
            .AddSortableField(SortBy.Date.ToString(), c => c.CreatedAt)
            .PaginateAsync(parameters.SortBy.ToString(), parameters.SortOrder, parameters.PageSize, parameters.Cursor, cancellationToken);

        return (items, nextCursor, hasNextPage);
    }
}
