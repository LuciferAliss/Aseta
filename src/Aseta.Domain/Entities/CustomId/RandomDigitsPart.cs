using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Aseta.Domain.Entities.CustomId;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(RandomDigitsPart), nameof(RandomDigitsPart))]
public record RandomDigitsPart(int Count, bool LeadingZeros, string Separator) : CustomIdPart(Separator)
{
    public override string GenerationCustomId()
    {
        int number = (int)Math.Pow(10, Count);
        return RandomNumberGenerator.GetInt32(0, number)
            .ToString(LeadingZeros ? "D" + Count : "");
    }

    public override bool IsValid(string customIdPart)
    {
        return int.TryParse(customIdPart, out _);
    }
}