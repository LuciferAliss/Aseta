using System.Text.Json.Serialization;

namespace Aseta.Domain.Entities.CustomId;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(DateTimePart), nameof(DateTimePart))]
public record DateTimePart(string Format, string Separator) : CustomIdPart(Separator)
{
    public override string GenerationCustomId()
    {
        return DateTime.UtcNow.ToString(Format);
    }

    public override bool IsValid(string customIdPart)
    {
        return DateTime.TryParse(customIdPart, out _);
    }
}