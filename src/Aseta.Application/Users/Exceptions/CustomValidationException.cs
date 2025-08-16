namespace Aseta.Application.Users.Exceptions;

public class CustomValidationException(Dictionary<string, string[]> errors) : Exception("Multiple errors occurred. See error details.")
{
    public Dictionary<string, string[]> Errors { get; set; } = errors;
}