using System;
using Aseta.Domain.Abstractions;

namespace Aseta.Application.Errors;

public class ItemServiceErrors
{
    public static readonly Error NotFoundInventory = new("ItemServiceErrors.NotFoundInventory", "Inventory not found.");
    public static readonly Error NotExistingInventory = new("ItemServiceErrors.NotExistingInventory", "Inventory does not exist.");
    public static readonly Error NotFoundUser = new("ItemServiceErrors.NotFoundUser", "User not found.");
    public static readonly Error NotFoundItem = new("ItemServiceErrors.NotFoundItem", "Item not found.");
}
