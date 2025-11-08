namespace Aseta.Application.Abstractions.Authorization;

public interface IInventoryScopedRequest
{
    Guid InventoryId { get; }
}