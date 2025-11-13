using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Mosaic.Configuration;

namespace Mosaic.Features.Email;

public class EmailRepository : IEmailRepository
{
    private readonly EmailSettings _emailSettings;

    public EmailRepository(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }

    public async Task SendNotificationToSubscribers(string message, string body = "")
    {
        foreach (string sub in _emailSettings.Subscribers)
        {
            var retryCount = 3;
            while (retryCount > 0)
            {
                if (await SendEmail(sub, message, body))
                {
                    break;
                }
                retryCount--;
            }
        }
    }

    private async Task<bool> SendEmail(string to, string subject, string body)
    {
        var message = new MailMessage(_emailSettings.SmtpSender, to)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
        {
            Credentials = new NetworkCredential(_emailSettings.SmtpSender, _emailSettings.SmtpPassword),
            EnableSsl = true
        };

        try
        {
            await client.SendMailAsync(message);
            return true;
        }
        catch (Exception _)
        {
            return false;
        }
    }
}
