namespace Mosaic.Features.Email;

public interface IEmailRepository
{
    Task SendNotificationToSubscribers(string message, string body = "");
}
