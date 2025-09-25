using System.Security.Claims;
using Aseta.Domain.Abstractions.Checkers;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.DTO.Category;
using Aseta.Domain.DTO.CustomField;
using Aseta.Domain.DTO.CustomId;
using Aseta.Domain.DTO.Inventory;
using Aseta.Domain.DTO.Item;
using Aseta.Domain.DTO.Tag;
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

    [HttpGet("get-inventory/{inventoryId}")]
    public async Task<ActionResult> GetInventories(Guid inventoryId)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        return Ok(await _inventoryService.GetInventoryAsync(inventoryId));
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

    [HttpPut("update-item/{inventoryId}")]
    public async Task<ActionResult> UpdateItem(Guid inventoryId, UpdateItemRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        if (!await _checkingAccessPolicy.CheckAsync(User, inventoryId, "CanEditInventory"))
            return Forbid("User not has permission");

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        await _inventoryService.UpdateItemAsync(request, inventoryId, Guid.Parse(userId));
        return Ok();
    }

    [HttpDelete("remove-item/{inventoryId}")]
    public async Task<ActionResult> RemoveItem([FromRoute] Guid inventoryId, [FromBody] RemoveItemsRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        if (!await _checkingAccessPolicy.CheckAsync(User, inventoryId, "CanEditInventory"))
            return Forbid("User not has permission");

        await _inventoryService.RemoveItemsAsync(request, inventoryId);
        return Ok();
    }

    [HttpGet("get-items/{inventoryId}")]
    public async Task<ActionResult> GetInventory([FromRoute] Guid inventoryId, [FromQuery] ItemViewRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);

        return Ok(await _inventoryService.GetItemAsync(request, inventoryId));
    }

    [HttpGet("get-last-inventories")]
    public async Task<ActionResult> GetLatsInventories([FromQuery] ViewLatestInventoryRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);

        return Ok(await _inventoryService.GetLastInventoriesAsync(request));
    }

    [HttpGet("get-most-popular-inventory/{count}")]
    public async Task<ActionResult> GetMostPopularInventories(int count)
    {
        await _checkingLockoutUser.CheckAsync(User);

        return Ok(await _inventoryService.GetMostPopularInventoriesAsync(count));
    }

    [HttpDelete("remove-inventory/{inventoryId}")]
    public async Task<ActionResult> RemoveInventory(Guid inventoryId)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        if (!await _checkingAccessPolicy.CheckAsync(User, inventoryId, "CanManageInventory"))
            return Forbid("User not has permission");

        await _inventoryService.RemoveInventoryAsync(inventoryId);
        return Ok();
    }

    [HttpGet("get-categories")]
    public async Task<ActionResult> GetAllCategories()
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        return Ok(await _inventoryService.GetAllCategoryAsync());
    }

    [Authorize]
    [HttpPost("create-inventory")]
    public async Task<ActionResult> CreateInventory(CreateInventoryRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        return Ok(await _inventoryService.CreateInventoryAsync(request, Guid.Parse(userId)));
    }

    [HttpGet("get-user-role-inventory/{id}")]
    [Authorize]
    public async Task<ActionResult> GetUserRoleInventory(Guid id)
    {
        await _checkingLockoutUser.CheckAsync(User);

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not found");

        return Ok(await _inventoryService.GetUserRoleInventoryAsync(id, Guid.Parse(userId)));
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

    [HttpGet("get-tags-cloud")]
    public async Task<ActionResult> GeTagsCloud()
    {
        await _checkingLockoutUser.CheckAsync(User);
        
        return Ok(await _inventoryService.GetTagsCloudAsync());
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

    [HttpPut("update-custom-id-rules/{inventoryId}")]
    public async Task<ActionResult> UpdateCustomIdRulePartsToInventory([FromRoute] Guid inventoryId, [FromBody] UpdateCustomIdPartsRequest request)
    {
        await _checkingLockoutUser.CheckAsync(User);

        if (!await _checkingAccessPolicy.CheckAsync(User, inventoryId, "CanManageInventory"))
            return Forbid("User not has permission");

        await _inventoryService.UpdateCustomIdRulePartsToInventoryAsync(request, inventoryId);

        return Ok();
    }
}