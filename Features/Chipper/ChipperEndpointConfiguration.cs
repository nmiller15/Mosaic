using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Mosaic.Features.Logging;
using Mosaic.Shared;

namespace Mosaic.Features.Chipper;

public static class ChipperEndpointConfiguration
{
    public static void ConfigureChipperEndpoints(this WebApplication app)
    {
        app.MapPost("/chipper", async (IChipperService chipperService, [FromBody] List<LogMessage> messages)
        => await chipperService.Log(messages));

        app.MapPost("/chipper/subscribe", async (IChipperService chipperService, [FromBody] AppSourceSubscriber subscriber)
            => await chipperService.UpdateSubscription(subscriber));
    }
}

public record EmailRequest()
{
    public string Email { get; set; }
}

