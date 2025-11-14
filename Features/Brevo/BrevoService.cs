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
        if (_emailService.ValidateEmail(email))
        {
            return Response<string>.Failure("Invalid email address.");
        }

        var id = await _brevoRepository.AddContact(new Contact
        {
            Email = email
        });

        var successMessage = "You were successfully subscribed!";

        if (id > 0)
        {
            await _emailService.SendNotificationToSubscribers($"New Subscriber: {email}",
                    "https://app.brevo.com/contact/list-listing/id/2");
            return Response<string>.Success(successMessage, successMessage);
        }

        return Response<string>.Failure("Failed to add contact to newsletter.");
    }
}
