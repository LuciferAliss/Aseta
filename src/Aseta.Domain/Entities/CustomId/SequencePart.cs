using System.Text.Json.Serialization;

namespace Aseta.Domain.Entities.CustomId;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(SequencePart), nameof(SequencePart))]
public record SequencePart(int Padding, int Number, string Separator) : CustomIdPart(Separator)
{
    public override string GenerationCustomId()
    {
        return Number.ToString("D" + Padding);
    }
}