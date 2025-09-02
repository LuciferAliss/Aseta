using System.Text.Json.Serialization;

namespace Aseta.Domain.Entities.CustomId;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(DateTimePart), nameof(DateTimePart))]
[JsonDerivedType(typeof(FixedTextPart), nameof(FixedTextPart))]
[JsonDerivedType(typeof(GuidPart), nameof(GuidPart))]
[JsonDerivedType(typeof(RandomDigitsPart), nameof(RandomDigitsPart))]
[JsonDerivedType(typeof(RandomNumberPart), nameof(RandomNumberPart))]
[JsonDerivedType(typeof(SequencePart), nameof(SequencePart))]
public abstract record CustomIdPart(string Separator)
{
    public abstract string GenerationCustomId();
    public abstract bool IsValid(string customIdPart);
}