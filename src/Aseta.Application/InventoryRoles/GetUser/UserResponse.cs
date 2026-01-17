namespace Aseta.Application.InventoryRoles.GetUser;

public record UserResponse(Guid UserId, string UserName, string Email, string Role);
