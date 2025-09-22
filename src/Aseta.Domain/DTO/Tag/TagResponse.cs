namespace Aseta.Domain.DTO.Tag;

public record TagResponse
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public int Weight { get; init; }
}