using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.Get.Contracts;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.InventoryRoles;
using AutoMapper;

namespace Aseta.Application.Inventories.Get;

internal sealed class GetInventoryQueryHandler(
    IInventoryRepository inventoryRepository,
    IInventoryUserRoleRepository inventoryUserRoleRepository) : IQueryHandler<GetInventoryQuery, InventoryResponse>
{
    public async Task<Result<InventoryResponse>> Handle(
        GetInventoryQuery query,
        CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(
            query.InventoryId,
            false,
            cancellationToken,
            i => i.Tags,
            i => i.UserRoles,
            i => i.Category,
            i => i.Creator);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(query.InventoryId);
        }

        if (!Uri.TryCreate(inventory.ImageUrl, UriKind.Absolute, out Uri? imageUrl) && imageUrl is null)
        {
            return InventoryErrors.ImageUrlNull();
        }

        Role userRoleInInventory = await inventoryUserRoleRepository.GetUserRoleInInventory(
            query.UserId,
            query.InventoryId,
            cancellationToken);

        var response = new InventoryResponse(
            inventory.Id,
            inventory.Name,
            inventory.Description,
            imageUrl,
            inventory.Creator.UserName,
            new CategoryResponse(inventory.Category.Id, inventory.Category.Name),
            inventory.IsPublic,
            inventory.CreatedAt,
            inventory.Tags.Select(t => new TagResponse(t.Id, t.Name)).ToList(),
            inventory.CustomFields.Select(c => new CustomFieldDefinitionResponse(c.Id, c.Name, c.Type.ToString())).ToList(),
            userRoleInInventory.ToString());

        return response;
    }
}