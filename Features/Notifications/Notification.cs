using System.Text.Json.Serialization;

namespace Mosaic.Features.Notifications;

public class Notification
{
    [JsonPropertyName("title")] public required string Title { get; set; }
    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
    [JsonPropertyName("priority")] public int Priority { get; set; }
}
