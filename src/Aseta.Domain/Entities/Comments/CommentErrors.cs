using System;
using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Comments;

public static class CommentErrors
{
    public static Error CommentNotFound() => Error.NotFound(
        "Comment.NotFound",
        "Comment not found.");

    public static Error ContentEmpty() => Error.Validation(
        "Comment.Content",
        "Content cannot be empty.");

    public static Error ContentTooLong(int contentLength) => Error.Validation(
        "Comment.Content",
        $"Content cannot be longer than {contentLength} characters.");
}