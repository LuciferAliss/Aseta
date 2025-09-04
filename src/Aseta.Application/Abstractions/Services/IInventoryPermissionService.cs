namespace Aseta.Application.Abstractions.Services;

public interface IInventoryPermissionService
{
    Task GrantEditorAccessAsync(Guid inventoryId, Guid userId);
    Task RevokeEditorAccessAsync(Guid inventoryId, Guid userId);
}