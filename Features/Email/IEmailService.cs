using Mosaic.Shared;

namespace Mosaic.Features.Email;

public interface IEmailService
{
    Task SendNotificationToSubscribers(string message, string body = "");
    Task<Response<bool>> SendEmail(Email email);
    Task<Response<bool>> SendEmailFromMosaicSender(Email email);
    bool ValidateEmail(string email);
}
