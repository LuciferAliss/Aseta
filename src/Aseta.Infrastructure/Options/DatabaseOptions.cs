namespace Aseta.Infrastructure.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";
    public string? Url { get; set; }
}
