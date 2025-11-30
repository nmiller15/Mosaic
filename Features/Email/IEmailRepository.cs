using MimeKit;

namespace Mosaic.Features.Email;

public interface IEmailRepository
{
    Task<EmailResult> SendEmail(Email email);
    Task<EmailResult> SendEmail(MimeMessage message);
    Task<List<EmailResult>> SendNotificationToSubscribers(string message, string body = "");
    Task<EmailResult> SendEmailFromMosaicSender(Email email);
}
