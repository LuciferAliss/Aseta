using System.Security.Claims;
using Aseta.Application.Abstractions.Checkers;
using Aseta.Application.Abstractions.Services;
using Aseta.Application.DTO.Category;
using Aseta.Application.DTO.CustomField;
using Aseta.Application.DTO.CustomId;
using Aseta.Application.DTO.Inventory;
using Aseta.Application.DTO.Item;
using Aseta.Application.DTO.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Controllers;

[Route("[controller]")]
[ApiController]
public class InventoryController(
    IInventoryService inventoryService,
    ICheckingAccessPolicy checkingAccessPolicy,
    ICheckingLockoutUser checkingLockoutUser
) : ControllerBase
{
    private readonly IInventoryService _inventoryService = inventoryService;
    private readonly ICheckingAccessPolicy _checkingAccessPolicy = checkingAccessPolicy;
    private readonly ICheckingLockoutUser _checkingLockoutUser = checkingLockoutUser;

    [HttpGet("get-inventory{InventoryId}")]
    public async Task<ActionResult> GetInventories(Guid InventoryId)
    {
        await _checkingLockoutUser.CheckAsync(User);

        if (!await _checkingAccessPolicy.CheckAsync(User, InventoryId, "CanViewInventory"))
            return Forbid("User not has permission");
        
        return Ok(await _inventoryService.GetInventoryAsync(InventoryId));
    }

    [HttpPost("create-item")]
    public async Task<ActionResult> CreateItem(CrateItemRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanEditInventory"))
            return Forbid("User not has permission");

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        await _inventoryService.AddItemAsync(request, Guid.Parse(userId));
        return Ok();
    }

    [HttpPut("update-item")]
    public async Task<ActionResult> UpdateItem(UpdateItemRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanEditInventory"))
            return Forbid("User not has permission");

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        await _inventoryService.UpdateItemAsync(request, Guid.Parse(userId));
        return Ok();
    }

    [HttpDelete("remove-item")]
    public async Task<ActionResult> RemoveItem(RemoveItemRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanEditInventory"))
            return Forbid("User not has permission");

        await _inventoryService.RemoveItemAsync(request);
        return Ok();
    }

    [HttpGet("get-items")]
    public async Task<ActionResult> GetInventory(ItemViewRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);

         var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? Guid.Empty.ToString();

        return Ok(await _inventoryService.GetItemAsync(request, Guid.Parse(userId)));
    }

    [HttpGet("get-inventories")]
    public async Task<ActionResult> GetInventories(InventoryViewRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? Guid.Empty.ToString();

        return Ok(await _inventoryService.GetPublicInventoriesAsync(request, Guid.Parse(userId)));
    }

    [HttpDelete("remove-inventory{InventoryId}")]
    public async Task<ActionResult> RemoveInventory(Guid InventoryId)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        if (!await _checkingAccessPolicy.CheckAsync(User, InventoryId, "CanManageInventory"))
            return Forbid("User not has permission");

        await _inventoryService.RemoveInventoryAsync(InventoryId);
        return Ok();
    }

    [Authorize]
    [HttpPost("create-inventory")]
    public async Task<ActionResult> CreateInventory(CreateInventoryRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        await _inventoryService.CreateInventoryAsync(request, Guid.Parse(userId));
        return Ok();
    }

    [HttpPut("update-inventory")]
    public async Task<ActionResult> UpdateInventory(UpdateInventoryRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanManageInventory"))
            return Forbid("User not has permission");

        await _inventoryService.UpdateInventoryAsync(request);
        return Ok();
    }

    [HttpPut("update-tags")]
    public async Task<ActionResult> UpdateTagsToInventory(UpdateInventoryTagsRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanEditInventory"))
            return Forbid("User not has permission");

        await _inventoryService.UpdateTagsToInventoryAsync(request);

        return Ok();
    }

    [HttpPut("update-category")]
    public async Task<ActionResult> UpdateCategoryToInventory(UpdateInventoryCategoryRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanManageInventory"))
            return Forbid("User not has permission");

        await _inventoryService.UpdateCategoryToInventoryAsync(request);

        return Ok();
    }

    [HttpPut("update-custom-fields")]
    public async Task<ActionResult> UpdateCustomFieldsToInventory(UpdateCustomFieldsRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
     
        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanManageInventory"))
            return Forbid("User not has permission");

        await _inventoryService.UpdateCustomFieldsToInventoryAsync(request);

        return Ok();
    }

    [HttpPut("update-custom-id-rule-parts")]
    public async Task<ActionResult> UpdateCustomIdRulePartsToInventory(UpdateCustomIdPartsRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);

        if (!await _checkingAccessPolicy.CheckAsync(User, request.InventoryId, "CanManageInventory"))
            return Forbid("User not has permission");

        await _inventoryService.UpdateCustomIdRulePartsToInventoryAsync(request);

        return Ok();
    }
}