using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Mosaic.Shared;

namespace Mosaic.Features.Turnstile;

public class TurnstileService : ITurnstileService
{
    private readonly HttpClient _client;
    private readonly TurnstileSettings _turnstileSettings;

    public TurnstileService(IHttpClientFactory httpClientFactory, IOptions<TurnstileSettings> turnstileSettings)
    {
        _client = httpClientFactory.CreateClient("Turnstile");
        _turnstileSettings = turnstileSettings.Value;
    }

    public async Task<Response<bool>> Verify(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Response<bool>.Failure("Turnstile verification is required.");
        }

        if (string.IsNullOrWhiteSpace(_turnstileSettings.SecretKey))
        {
            return Response<bool>.Failure("Turnstile secret is not configured.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["secret"] = _turnstileSettings.SecretKey,
                ["response"] = token,
            })
        };

        using var response = await _client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return Response<bool>.Failure("Turnstile verification failed.");
        }

        var payload = await response.Content.ReadFromJsonAsync<TurnstileVerificationResponse>();
        if (payload is null)
        {
            return Response<bool>.Failure("Turnstile verification failed.");
        }

        return payload.Success
            ? Response<bool>.Success(true)
            : Response<bool>.Failure("Turnstile verification failed.");
    }
}
