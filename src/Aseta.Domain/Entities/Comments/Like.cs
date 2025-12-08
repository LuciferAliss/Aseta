using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Comments;

public class Like : Entity
{
    public Guid CommentId { get; private set; }
    public virtual Comment Comment { get; }
    public Guid UserId { get; private set; }
    public virtual User User { get; }
    public DateTime CreatedAt { get; private set; }

    private Like() { }

    private Like(Guid id, Guid commentId, Guid userId, DateTime createdAt) : base(id)
    {
        CommentId = commentId;
        UserId = userId;
        CreatedAt = createdAt;
    }

    public static Result<Like> Create(Guid commentId, Guid userId)
    {
        return new Like(Guid.NewGuid(), commentId, userId, DateTime.UtcNow);
    }
}