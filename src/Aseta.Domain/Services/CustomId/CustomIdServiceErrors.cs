using Aseta.Domain.Abstractions;

namespace Aseta.Domain.Services.CustomId;

public static class CustomIdServiceErrors
{
    public static readonly Error InventoryIdEmpty = new("CustomIdServiceErrors.InventoryIdEmpty", "Inventory id is empty.", ErrorType.NotFound);
    public static readonly Error CustomIdEmpty = new("CustomIdServiceErrors.CustomIdEmpty", "Custom id is empty.", ErrorType.NotFound);
}