using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Application.Services;

public class InventoryPermissionService(
    IInventoryUserRoleRepository inventoryUserRoleRepository,
    IInventoryRepository inventoryRepository,
    UserManager<UserApplication> userManager,
    IUnitOfWork unitOfWork
) : IInventoryPermissionService
{
    private readonly IInventoryUserRoleRepository _inventoryUserRoleRepository = inventoryUserRoleRepository;
    private readonly IInventoryRepository _inventoryRepository = inventoryRepository;
    private readonly UserManager<UserApplication> _userManager = userManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task GrantEditorAccessAsync(Guid inventoryId, Guid userId)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(inventoryId)
            ?? throw new Exception("Inventory not found");

        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        await _inventoryUserRoleRepository.AddAsync(InventoryUserRole.Create(user.Id, inventory.Id, InventoryRoleType.Editor));

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RevokeEditorAccessAsync(Guid inventoryId, Guid userId)
    {
        var inventory = await _inventoryRepository.GetByIdAsync(inventoryId)
            ?? throw new Exception("Inventory not found");

        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        var userRole = await _inventoryUserRoleRepository.GetUserGrantToInventoryAsync(user.Id, inventory.Id, InventoryRoleType.Editor)
            ?? throw new Exception("Role not found");

        await _inventoryUserRoleRepository.DeleteAsync(userRole);

        await _unitOfWork.SaveChangesAsync();
    }
}
