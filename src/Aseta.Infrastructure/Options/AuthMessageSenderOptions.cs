using System;

namespace Aseta.Infrastructure.Options;

public sealed record AuthMessageSenderOptions
{
    public const string SEND_GRID_KEY = "AuthMessageSender";

    public required string SendGridKey { get; set; }
}
