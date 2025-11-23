using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories.CustomId;

namespace Aseta.Domain.Abstractions.Services;

public interface ICustomIdService
{
    Task<Result<string>> GenerateAsync(Guid inventoryId, Guid itemID, ICollection<CustomIdRuleBase> customIdRule, CancellationToken cancellationToken = default);
    Result<bool> IsValid(string customId, ICollection<CustomIdRuleBase> customIdRule);
}