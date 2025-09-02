using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Aseta.Domain.Entities.CustomId;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(RandomNumberPart), nameof(RandomNumberPart))]
public record RandomNumberPart(int Bits, string FormatOption, string Separator) : CustomIdPart(Separator)
{
    public override string GenerationCustomId()
    {
        byte[] randomBytes = new byte[Bits / 8];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return BitConverter.ToUInt32(randomBytes, 0).ToString(FormatOption);
    }

    public override bool IsValid(string customIdPart)
    {
        return int.TryParse(customIdPart, out _);
    }
}
