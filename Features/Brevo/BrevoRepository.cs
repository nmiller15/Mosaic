using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mosaic.Shared;

namespace Mosaic.Features.Brevo;

public class BrevoRepository : IBrevoRepository
{
    private readonly HttpClient _client;

    private const int RETRY_LIMIT = 5;
    private const int PRIMARY_LIST_ID = 2;

    private readonly List<HttpStatusCode> RetryCodes = new List<HttpStatusCode>()
     {
         HttpStatusCode.Conflict,
         HttpStatusCode.TooManyRequests,
         HttpStatusCode.InternalServerError,
         HttpStatusCode.BadGateway,
         HttpStatusCode.ServiceUnavailable,
         HttpStatusCode.GatewayTimeout
     };

    public BrevoRepository(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("Brevo");
    }

    public async Task<int> AddContact(Contact contact)
    {
        var requestUri = BuildRequestUri("contacts");
        var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
        request.AddModel<BrevoContact>(new BrevoContact
        {
            Email = contact.Email,
            ListIds = [PRIMARY_LIST_ID],
            UpdateEnabled = true
        });

        var response = await Send(request);


        if (response.StatusCode != HttpStatusCode.Created)
        {
            return -1;
        }


        var data = JsonSerializer.Deserialize<BrevoContactCreatedResponse>(await response.Content.ReadAsStringAsync());
        return data.Id;
    }

    private string BuildRequestUri(string resourceType)
    {
        var builder = new StringBuilder();
        builder.Append(resourceType);
        return builder.ToString();
    }

    private async Task<HttpResponseMessage?> Send(HttpRequestMessage request)
    {
        var response = await _client.SendAsync(request);

        var retries = 0;
        var sleepMs = 1000;

        while (!response.IsSuccessStatusCode &&
               RetryCodes.Contains(response.StatusCode) &&
               retries < RETRY_LIMIT)
        {
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                Thread.Sleep(sleepMs);
                sleepMs *= 2;
            }

            response = await _client.SendAsync(request);
            retries++;
        }

        return response;
    }
}

struct BrevoContactCreatedResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}

struct BrevoFailureRepsonse
{
    public string Code { get; set; }
    public string Message { get; set; }

    public void Dump()
    {
        Console.WriteLine("BrevoFailureResponse: { Code: " +
                Code + ", Message: " + Message + " }");
    }
}
