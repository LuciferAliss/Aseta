using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.CustomId;

namespace Aseta.Domain.Abstractions.Services;

public interface ICustomIdService
{
    Task<Result<string>> GenerateAsync(ICollection<CustomIdRuleBase> customIdRule, Guid inventoryId, CancellationToken cancellationToken = default);
    Result<bool> IsValid(string customId, ICollection<CustomIdRuleBase> customIdRule);
}