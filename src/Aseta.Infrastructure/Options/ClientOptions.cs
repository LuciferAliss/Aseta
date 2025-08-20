namespace Aseta.Infrastructure.Options;

public sealed class ClientOptions
{
    public const string SectionName = "Client";

    public string? Url { get; set; }
}
