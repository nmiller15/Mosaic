namespace Mosaic.Features.Email;

public interface IEmailService
{
    Task SendNotificationToSubscribers(string message, string body = "");
}
