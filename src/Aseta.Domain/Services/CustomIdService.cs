using System.Text;
using System.Text.RegularExpressions;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.CustomId;

namespace Aseta.Domain.Services;

public class CustomIdService : ICustomIdService
{
    public string GenerateAsync(List<CustomIdPart> customFields)
    {
        if (customFields.Count == 0) return Guid.NewGuid().ToString();

        string customId = "";

        foreach (var item in customFields)
        {
            customId += item.GenerationCustomId() + item.Separator;
        }

        return customId[..^1];
    }

    public bool IsValid(string customId, List<CustomIdPart> customIdParts)
    {
        return true;
    }
}