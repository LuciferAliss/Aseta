using Aseta.Application.Abstractions.Services;
using Aseta.Application.DTO.Inventory;
using Aseta.Domain.Entities.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Controllers;

[Route("[controller]")]
[ApiController]
public class InventoryController(IInventoryService inventoryService, IAuthorizationService authorizationService) : ControllerBase
{
    private readonly IInventoryService _inventoryService = inventoryService;
    private readonly IAuthorizationService _authorizationService = authorizationService;

    [HttpPost("create-item")]
    public async Task<ActionResult> CreateItem(CrateItemRequest request)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(
            User,
            request.InventoryId,
            "CanEditInventory"
        );

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        await _inventoryService.AddItemAsync(request);
        return Ok();
    }

    [HttpDelete("remove-item")]
    public async Task<ActionResult> RemoveItem(RemoveItemRequest request)
    {
        await _inventoryService.RemoveItemAsync(request);
        return Ok();
    }

    [HttpDelete("remove-inventory")]
    public async Task<ActionResult> RemoveInventory(RemoveInventoryRequest request)
    {
        await _inventoryService.RemoveInventoryAsync(request);
        return Ok();
    }

    [HttpPost("create-inventory")]
    public async Task<ActionResult> CreateInventory(CreateInventoryRequest request)
    {
        await _inventoryService.CreateInventoryAsync(request);
        return Ok();
    }
}

