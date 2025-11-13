using Microsoft.AspNetCore.Mvc;

namespace Mosaic.Features.Brevo;

public static class BrevoEndpointConfiguration
{
    public static void ConfigureBrevoEndpoints(this WebApplication app)
    {
        app.MapPost("/newsletter/", async (IBrevoService brevoService, [FromBody] EmailRequest request)
            => await brevoService.AddToNewsletter(request.Email));
    }
}

public record EmailRequest()
{
    public string Email { get; set; }
}
