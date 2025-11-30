using Microsoft.Extensions.Options;
using Mosaic.Configuration;
using Mosaic.Features.Email;

namespace Mosaic.Features.Things;

public class ThingsRepository : IThingsRepository
{
    private readonly IEmailService _emailService;
    private readonly ThingsSettings _thingsSettings;

    public ThingsRepository(IEmailService emailService, IOptions<ThingsSettings> thingsSettings)
    {
        _emailService = emailService;
        _thingsSettings = thingsSettings.Value;
    }

    public async Task<bool> AddToThings(ThingsItem todo)
    {
        var email = new Email.Email();
        email.Subject = todo.Title;
        email.Body = todo.Description;

        email.To = new EmailAddress
        {
            DisplayName = "Add To Things",
            Address = _thingsSettings.AddToThingsAddress,
        };

        var response = await _emailService.SendEmailFromMosaicSender(email);
        return response.WasSuccessful;
    }
}
