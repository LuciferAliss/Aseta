
using Aseta.Domain.Entities.CustomId;

namespace Aseta.Application.DTO.CustomId;

public record UpdateCustomIdPartsRequest(Guid InventoryId, List<CustomIdRuleBase> CustomIdParts);