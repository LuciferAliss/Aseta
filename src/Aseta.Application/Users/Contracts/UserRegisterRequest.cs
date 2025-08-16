namespace Aseta.Application.Users.Contracts;

public sealed record UserRegisterRequest(string UserName, string Email, string Password);