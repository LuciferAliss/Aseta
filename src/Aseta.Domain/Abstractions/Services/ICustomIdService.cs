using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories.CustomId;

namespace Aseta.Domain.Abstractions.Services;

public interface ICustomIdService
{
    Task<Result<string>> GenerateAsync(Guid inventoryId, Guid itemId, ICollection<CustomIdRuleBase> customIdRule, CancellationToken cancellationToken = default);
    Result<bool> IsValid(string customId, ICollection<CustomIdRuleBase> customIdRule);
}