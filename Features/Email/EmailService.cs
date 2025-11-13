namespace Mosaic.Features.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;

        public EmailService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public async Task SendNotificationToSubscribers(string message, string body = "")
        => await _emailRepository.SendNotificationToSubscribers(message, body);
    }
}
