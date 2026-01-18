namespace Aseta.Application.Comments.GetPaginated.Contracts;

public sealed record CommentResponse(
    Guid Id,
    string Content,
    string UserName,
    string Email,
    DateTime CreatedAt);