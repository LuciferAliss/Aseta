using System.Security.Claims;
using Aseta.Application.Abstractions.Checkers;
using Aseta.Application.Abstractions.Services;
using Aseta.Application.DTO.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Controllers;

[Route("[controller]")]
[ApiController]
public class InventoryController(IInventoryService inventoryService,
    ICheckingAccessPolicy checkingAccessPolicy,
    ICheckingLockoutUser checkingLockoutUser) : ControllerBase
{
    private readonly IInventoryService _inventoryService = inventoryService;
    private readonly ICheckingAccessPolicy _checkingAccessPolicy = checkingAccessPolicy;
    private readonly ICheckingLockoutUser _checkingLockoutUser = checkingLockoutUser;

    [HttpPost("create-item")]
    public async Task<ActionResult> CreateItem(CrateItemRequest request)
    {
        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanEditInventory"))
            return Forbid("User not has permission");

        if (await _checkingLockoutUser.CheckAsync(User))
            return Forbid("User is lockout");

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        await _inventoryService.AddItemAsync(request, Guid.Parse(userId));
        return Ok();
    }

    [HttpPut("update-item")]
    public async Task<ActionResult> UpdateItem(UpdateItemRequest request)
    {
        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanEditInventory"))
            return Forbid("User not has permission");

        if (await _checkingLockoutUser.CheckAsync(User))
            return Forbid("User is lockout");

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        await _inventoryService.UpdateItemAsync(request, Guid.Parse(userId));
        return Ok();
    }

    [HttpDelete("remove-item")]
    public async Task<ActionResult> RemoveItem(RemoveItemRequest request)
    {
        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanEditInventory"))
            return Forbid("User not has permission");

        if (await _checkingLockoutUser.CheckAsync(User))
            return Forbid("User is lockout");

        await _inventoryService.RemoveItemAsync(request);
        return Ok();
    }

    [HttpDelete("remove-inventory{InventoryId}")]
    public async Task<ActionResult> RemoveInventory(Guid InventoryId)
    {
        if (!await _checkingAccessPolicy.CheckAsync(User, InventoryId, "CanManageInventory"))
            return Forbid("User not has permission");

        if (await _checkingLockoutUser.CheckAsync(User))
            return Forbid("User is lockout");

        await _inventoryService.RemoveInventoryAsync(InventoryId);
        return Ok();
    }

    [Authorize]
    [HttpPost("create-inventory")]
    public async Task<ActionResult> CreateInventory(CreateInventoryRequest request)
    {
        if (await _checkingLockoutUser.CheckAsync(User))
            return Forbid("User is lockout");

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        await _inventoryService.CreateInventoryAsync(request, Guid.Parse(userId));
        return Ok();
    }
}