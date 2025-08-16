namespace Aseta.Domain.User.Exceptions;

public class UserAlreadyExistsException(string? message) : Exception(message);