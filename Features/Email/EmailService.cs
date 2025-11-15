using System.Text.RegularExpressions;

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

        public bool ValidateEmail(string email)
        {
            var pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            Console.WriteLine($"Validating email: {email}");
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
    }
}
