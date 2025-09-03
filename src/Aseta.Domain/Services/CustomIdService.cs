using System.Text;
using System.Text.RegularExpressions;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.CustomId;

namespace Aseta.Domain.Services;

public class CustomIdService : ICustomIdService
{//IGenerationContext    
    public string GenerateAsync(List<CustomIdRuleBase> customIdRule)
    {
        if (customIdRule.Count == 0) return Guid.NewGuid().ToString();

        var customId = customIdRule.Select(r => r.Generation());
        return string.Join("-", customId);
    }

    public bool IsValid(string customId, List<CustomIdRuleBase> customIdRule)
    {
        var parts = customId.Split('-');
        if (parts.Length != customIdRule.Count) return false;

        for (int i = 0; i < parts.Length; i++)
        {
            if (!customIdRule[i].IsValid(parts[i]))
                return false;
        }

        return true;
    }
}