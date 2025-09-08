using Aseta.Application.DTO.CustomField;
using Aseta.Application.DTO.User;

namespace Aseta.Application.DTO.Item;

public record ItemResponse
(
    Guid Id,
    string CustomId,
    List<CustomFieldValueResponse> CustomFields, 
    UserInventoryInfoResponse UserUpdate,
    UserInventoryInfoResponse UserCreate,
    DateTime CreatedAt,
    DateTime UpdatedAt
);