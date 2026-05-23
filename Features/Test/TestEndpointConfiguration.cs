using Microsoft.AspNetCore.Mvc;

namespace Mosaic.Features.Test;

public static class TestEndpointConfiguration
{
    public static void ConfigureTestEndpoints(this WebApplication app)
    {
        app.MapGet("/Test/", () => Results.Ok(new { message = "request successful" }));
    }
}
