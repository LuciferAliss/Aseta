using System;
using Aseta.Domain.Entities.Items;

namespace Aseta.Application.DTO.Inventory;

public record CrateItemRequest(Guid InventoryId, List<CustomField> CustomFields, Guid CreatorId);