using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.Get.Contracts;

namespace Aseta.Application.Inventories.Get;

public sealed record GetInventoryQuery(Guid InventoryId, Guid UserId) : IQuery<InventoryResponse>;
