using Mosaic.Features.Brevo;
using Mosaic.Features.Things;

namespace Mosaic.Configuration;

public static class EndpointConfig
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        app.MapGet("/health", () => "Mosaic API is running.");

        app.ConfigureBrevoEndpoints();
        app.ConfigureThingsEndpoints();
    }
}
