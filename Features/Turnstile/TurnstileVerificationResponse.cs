using System.Text.Json.Serialization;

namespace Mosaic.Features.Turnstile;

public class TurnstileVerificationResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
