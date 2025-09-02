using System.Text.Json.Serialization;

namespace Aseta.Domain.Entities.CustomId;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(GuidPart), nameof(GuidPart))]
public record GuidPart(string FormatOption, string Separator) : CustomIdPart(Separator)
{
    public override string GenerationCustomId() => Guid.NewGuid().ToString(FormatOption);

    public override bool IsValid(string customIdPart)
    {
        return Guid.TryParse(customIdPart, out _);
    }
}