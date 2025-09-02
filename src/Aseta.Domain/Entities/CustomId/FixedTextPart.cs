using System.Text.Json.Serialization;

namespace Aseta.Domain.Entities.CustomId;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(FixedTextPart), nameof(FixedTextPart))]
public record FixedTextPart(string Value, string Separator) : CustomIdPart(Separator)
{
    public override string GenerationCustomId() => Value;

    public override bool IsValid(string customIdPart)
    {
        return true;
    }
}