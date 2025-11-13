using System.Text;
using System.Text.Json;

namespace Mosaic.Shared;

public static class HttpRequestMessageExtensions
{
    public static void AddModel<T>(this HttpRequestMessage message, T model)
    {
        if (!message.Method.Equals(HttpMethod.Post) &&
                !message.Method.Equals(HttpMethod.Put))
        {
            throw new InvalidOperationException("Models can only be added to POST or PUT requests.");
        }

        message.Headers.Add("accept", "application/json");

        message.Content = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            "application/json");
    }
}
