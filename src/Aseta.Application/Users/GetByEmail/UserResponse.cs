namespace Aseta.Application.Users.GetByEmail;

public record UserResponse(Guid Id, string UserName, string Email);
