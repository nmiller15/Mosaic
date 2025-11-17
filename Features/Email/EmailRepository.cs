using System.Net;
using Microsoft.Extensions.Options;
using MimeKit;
using Mosaic.Configuration;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Mosaic.Features.Email;

public class EmailRepository : IEmailRepository
{
    private readonly EmailSettings _emailSettings;

    public EmailRepository(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }

    public async Task<List<EmailResult>> SendNotificationToSubscribers(string message, string body = "")
    {
        var results = new List<EmailResult>();
        foreach (var sub in _emailSettings.Subscribers)
        {
            var email = new MimeMessage()
            {
                Subject = message,
                Body = new BodyBuilder() { HtmlBody = body, }.ToMessageBody(),
            };
            email.From.Add(new MailboxAddress("Mosaic", _emailSettings.SmtpSender));
            email.To.Add(new MailboxAddress(null, sub));

            var result = await SendEmail(email);
            results.Add(result);
        }
        return results;
    }

    public async Task<EmailResult> SendEmail(MimeMessage email)
    {
        NetworkCredential credentials = new()
        {
            UserName = _emailSettings.SmtpUser,
            Password = _emailSettings.SmtpPassword
        };

        using SmtpClient client = new();
        try
        {
            await client.ConnectAsync(
                    _emailSettings.SmtpServer,
                    _emailSettings.SmtpPort,
                    MailKit.Security.SecureSocketOptions.StartTls
                );
            await client.AuthenticateAsync(credentials);
            await client.SendAsync(email);
            return new EmailResult
            {
                WasSuccessful = true,
                Message = string.Empty
            };
        }
        catch (Exception ex)
        {
            return new EmailResult
            {
                WasSuccessful = false,
                Message = ex.Message
            };
        }
    }
}
