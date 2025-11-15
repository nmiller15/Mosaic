using System.Text.RegularExpressions;
using Mosaic.Features.Email;
using Mosaic.Shared;

namespace Mosaic.Features.Brevo;

public class BrevoService : IBrevoService
{
    private readonly IBrevoRepository _brevoRepository;
    private readonly IEmailService _emailService;

    public BrevoService(IBrevoRepository brevoRepository, IEmailService emailService)
    {
        _brevoRepository = brevoRepository;
        _emailService = emailService;
    }

    public async Task<Response<string>> AddToNewsletter(string email)
    {
        if (!_emailService.ValidateEmail(Normalize(email)))
        {
            return Response<string>.Failure("Invalid email address.");
        }

        // Ask brevo if contact exists?

        var id = await _brevoRepository.AddContact(new Contact
        {
            Email = email
        });

        var successMessage = "You were successfully subscribed!";

        if (id < 0)
        {
            return Response<string>.Failure(
                "Failed to add contact to newsletter.");
        }

        await _emailService.SendNotificationToSubscribers($"New Subscriber: {email}",
                "https://app.brevo.com/contact/list-listing/id/2");
        return Response<string>.Success(successMessage, successMessage);
    }

    private string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Special characters
        input = Regex.Replace(input, @"\p{S}", string.Empty);

        // White space
        input = Regex.Replace(input, @"\s+", string.Empty);

        return input;
    }
}
