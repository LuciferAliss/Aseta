namespace Aseta.Infrastructure.Options;

public sealed class GoogleAuthOptions
{
    public const string SectionName = "Google:Authentication";

    public string? Id { get; set; }
    public string? Secret { get; set; }
}