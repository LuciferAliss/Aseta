using Aseta.Infrastructure.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Aseta.Infrastructure.Services;

internal sealed class EmailSender(
    IOptions<AuthMessageSenderOptions> optionsAccessor) : IEmailSender
{
    private readonly AuthMessageSenderOptions _options = optionsAccessor.Value;

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(_options.Key))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(_options.Key, subject, message, toEmail);
    }

    private static async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("aseta.distribution.app@gmail.com", "Aseta"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        msg.SetClickTracking(false, false);
        await client.SendEmailAsync(msg);
    }
}