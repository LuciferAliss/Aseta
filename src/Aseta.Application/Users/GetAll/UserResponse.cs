namespace Aseta.Application.Users.GetAll;

public record class UserResponse(Guid Id, string UserName, string Email, bool IsLocked, string Role);
