using Mosaic.Features.Brevo;

namespace Mosaic.Configuration;

public static class EndpointConfig
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        app.MapGet("/health", () => "Mosaic API is running.");

        app.ConfigureBrevoEndpoints();
    }
}
