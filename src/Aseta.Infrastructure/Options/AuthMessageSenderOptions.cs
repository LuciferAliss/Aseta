namespace Aseta.Infrastructure.Options;

public sealed record AuthMessageSenderOptions
{
    public const string SectionName = "SendGrid";

    public string? Key { get; set; }
}
