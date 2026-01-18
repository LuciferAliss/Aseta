namespace Aseta.Application.Users.GetAll;

public record class UsersResponse(ICollection<UserResponse> Users);
