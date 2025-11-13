using System.Text.Json.Serialization;

namespace Mosaic.Features.Brevo;

public class BrevoContact
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("listIds")]
    public List<int> ListIds { get; set; }
    [JsonPropertyName("updateEnabled")]
    public bool UpdateEnabled { get; set; } = true;
}
