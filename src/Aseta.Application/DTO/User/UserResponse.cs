using Aseta.Application.DTO.Inventory;

namespace Aseta.Application.DTO.User;

public record UserResponse
(
    Guid Id,
    string Email,
    List<InventoryInfoProfileResponse> UserOwnerInventories,
    List<InventoryInfoProfileResponse> UserEditorInventories
);