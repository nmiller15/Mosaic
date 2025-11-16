using Microsoft.AspNetCore.Mvc;
using Mosaic.Features.Email;

namespace Mosaic.Features.Brevo;

public static class BrevoEndpointConfiguration
{
    public static void ConfigureBrevoEndpoints(this WebApplication app)
    {
        app.MapPost("/newsletter/", async (IBrevoService brevoService, [FromBody] EmailRequest request)
            => await brevoService.AddToNewsletter(request.Email));

        app.MapGet("/test/", async (IEmailService emailService) => await emailService.SendNotificationToSubscribers("Test Subject", "This is a test email body."));
    }
}

public record EmailRequest()
{
    public string Email { get; set; }
}
