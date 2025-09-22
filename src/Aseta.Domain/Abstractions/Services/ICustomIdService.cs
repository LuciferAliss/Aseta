using Aseta.Domain.Entities.CustomId;

namespace Aseta.Domain.Abstractions.Services;

public interface ICustomIdService
{
    Task<Result<string>> GenerateAsync(List<CustomIdRuleBase> customIdRule, Guid itemId);
    Result<bool> IsValid(string customId, List<CustomIdRuleBase> customIdRule);
}
