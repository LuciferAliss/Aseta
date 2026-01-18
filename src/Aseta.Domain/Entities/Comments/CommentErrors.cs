using System;
using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Comments;

public static class CommentErrors
{
    public static Error NotFound(Guid commentId) => Error.NotFound(
        "Comment.NotFound",
        $"The comment with id: {commentId} was not found.");

    public static Error CommentNotFound() => Error.NotFound(
        "Comment.NotFound",
        "Comment not found.");

    public static Error ContentEmpty() => Error.Validation(
        "Comment.Content",
        "Content cannot be empty.");

    public static Error ContentTooLong(int contentLength) => Error.Validation(
        "Comment.Content",
        $"Content cannot be longer than {contentLength} characters.");

    public static Error NotOwner(Guid commentId, Guid userId) => Error.Forbidden(
        "Comment.NotOwner",
        $"The comment with id: {commentId} does not belong to the user with id: {userId}.");
}