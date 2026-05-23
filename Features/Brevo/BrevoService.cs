using System.Text.RegularExpressions;
using Mosaic.Features.Email;
using Mosaic.Features.Turnstile;
using Mosaic.Shared;

namespace Mosaic.Features.Brevo;

public class BrevoService : IBrevoService
{
    private readonly IBrevoRepository _brevoRepository;
    private readonly IEmailService _emailService;
    private readonly ITurnstileService _turnstileService;

    public BrevoService(
        IBrevoRepository brevoRepository,
        IEmailService emailService,
        ITurnstileService turnstileService)
    {
        _brevoRepository = brevoRepository;
        _emailService = emailService;
        _turnstileService = turnstileService;
    }

    public async Task<Response<string>> AddToNewsletter(string email, string turnstileToken)
    {
        var normalizedEmail = Normalize(email);

        if (!_emailService.ValidateEmail(normalizedEmail))
        {
            return Response<string>.Failure("Invalid email address.");
        }

        if (string.IsNullOrWhiteSpace(turnstileToken))
        {
            return Response<string>.Failure("Turnstile verification is required.");
        }

        var turnstileResponse = await _turnstileService.Verify(turnstileToken);
        if (!turnstileResponse.WasSuccessful || !turnstileResponse.Data)
        {
            return Response<string>.Failure("Turnstile verification failed.");
        }

        // Ask brevo if contact exists?

        var id = await _brevoRepository.AddContact(new Contact
        {
            Email = normalizedEmail
        });

        var successMessage = "You were successfully subscribed!";

        if (id < 0)
        {
            return Response<string>.Failure(
                "Failed to add contact to newsletter.");
        }

        await _emailService.SendNotificationToSubscribers($"New Subscriber: {normalizedEmail}",
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
