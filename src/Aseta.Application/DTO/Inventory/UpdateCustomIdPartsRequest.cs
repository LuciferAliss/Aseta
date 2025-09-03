
using Aseta.Domain.Entities.CustomId;

namespace Aseta.Application.DTO.Inventory;

public record UpdateCustomIdPartsRequest(Guid InventoryId, List<CustomIdRuleBase> CustomIdParts);