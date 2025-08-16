namespace Aseta.Application.Users.Contracts;

public sealed record UserLoginRequest(string UserName, string Password);