using Aseta.Domain.Abstractions.Persistence;

namespace Aseta.Domain.Entities.CustomId;

public class GenerationContext(DateTime generationTime, int itemSequence)
{

    public DateTime GenerationTime { get; } = generationTime;
    public int ItemSequence { get; } = itemSequence;
}