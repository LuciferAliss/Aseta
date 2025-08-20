namespace Aseta.Infrastructure.Options;

public sealed record AuthMessageSenderOptions
{
    public const string SectionName = "Sendgrid";

    public string? Key { get; set; }
}
