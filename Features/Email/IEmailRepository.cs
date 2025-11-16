using System.Net.Mail;

namespace Mosaic.Features.Email;

public interface IEmailRepository
{
    Task SendEmail(MailMessage message);
    Task SendNotificationToSubscribers(string message, string body = "");
}
