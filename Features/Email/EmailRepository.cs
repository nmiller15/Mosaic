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

    public async Task SendEmail(MailMessage message)
    {
        var client = new SmtpClient
        {
            Host = _emailSettings.SmtpServer,
            Port = _emailSettings.SmtpPort,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPassword)
        };
        message.From = new MailAddress(_emailSettings.SmtpSender, "Mosaic Notification");

        await client.SendMailAsync(message);
    }

    public async Task SendNotificationToSubscribers(string message, string body = "")
    {
        foreach (string sub in _emailSettings.Subscribers)
        {
            var mailMessage = new MailMessage
            {
                Subject = message,
                Body = body
            };

            mailMessage.To.Add(sub);

            await SendEmail(mailMessage);
        }
    }

}
