using Microsoft.AspNetCore.Mvc;

namespace Mosaic.Features.Things;

public static class ThingsEndpointConfiguration
{
    public static void ConfigureThingsEndpoints(this WebApplication app)
    {
        app.MapPost("/Things/Add",
                async (IThingsService thingsService, [FromBody] ThingsItem todo)
                => await thingsService.AddToThings(todo))
        .RequireAuthorization();
    }
}
