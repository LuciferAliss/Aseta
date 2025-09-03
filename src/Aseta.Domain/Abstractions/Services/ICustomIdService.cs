using Aseta.Domain.Entities.CustomId;

namespace Aseta.Domain.Abstractions.Services;

public interface ICustomIdService
{
    string GenerateAsync(List<CustomIdRuleBase> customFields);
}
