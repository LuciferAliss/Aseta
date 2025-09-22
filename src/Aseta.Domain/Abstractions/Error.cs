namespace Aseta.Domain.Abstractions;

public record Error(string Code, string? Description = null)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided.");

    public static implicit operator Result(Error error) => Result.Failure(error);
}