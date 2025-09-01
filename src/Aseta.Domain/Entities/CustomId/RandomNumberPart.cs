using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Aseta.Domain.Entities.CustomId;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(RandomNumberPart), nameof(RandomNumberPart))]
public record RandomNumberPart(int Bits, string FormatOption, string Separator) : CustomIdPart(Separator)
{
    public override string GenerationCustomId()
    {
        return RandomNumberGenerator.GetInt32(1 << Bits).ToString(FormatOption);
    }
}
