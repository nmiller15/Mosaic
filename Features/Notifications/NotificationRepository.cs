using Mosaic.Shared;

namespace Mosaic.Features.Notifications;

public class NotificationRepository : INotificationRepository
{
    private readonly HttpClient _client;

    public NotificationRepository(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("Gotify");
    }

    public async Task Send(Notification notification)
    {
        var requestUri = "/message";
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "title", notification.Title },
            { "message", notification.Message },
            { "priority", notification.Priority.ToString() }
        });

        var response = await _client.PostAsync(requestUri, content);
        Console.WriteLine(response.StatusCode);
    }
}
