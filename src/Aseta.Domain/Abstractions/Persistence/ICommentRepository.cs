using Aseta.Domain.DTO.Comments;
using Aseta.Domain.Entities.Comments;

namespace Aseta.Domain.Abstractions.Persistence;

public interface ICommentRepository : IRepository<Comment>
{
    Task<(ICollection<Comment> comments, string? nextCursor, bool hasNextPage)> GetPaginatedWithKeysetAsync(
        CommentPaginationParameters parameters,
        Guid inventoryId,
        CancellationToken cancellationToken);
}
