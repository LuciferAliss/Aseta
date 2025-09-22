using Aseta.Domain.Abstractions;

namespace Aseta.Domain.Errors;

public static class CustomIdServiceErrors
{
    public static readonly Error InventoryIdEmpty = new("CustomIdServiceErrors.InventoryIdEmpty", "Inventory id is empty.");
    public static readonly Error CustomIdEmpty = new("CustomIdServiceErrors.CustomIdEmpty", "Custom id is empty.");
}