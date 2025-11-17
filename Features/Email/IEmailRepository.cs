using MimeKit;

namespace Mosaic.Features.Email;

public interface IEmailRepository
{
    Task<EmailResult> SendEmail(MimeMessage message);
    Task<List<EmailResult>> SendNotificationToSubscribers(string message, string body = "");
}
