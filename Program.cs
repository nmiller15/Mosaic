using Microsoft.AspNetCore.Authentication;
using Mosaic;
using Mosaic.Auth;
using Mosaic.Configuration;
using Mosaic.Features.Logging;
using Mosaic.Features.Turnstile;
using Mosaic.Shared;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configure();
        builder.ConfigureApplicationWhitelist();
        builder.ConfigureEmailSettings();
        builder.ConfigureThingsSettings();
        builder.ConfigureTurnstileSettings();
        builder.ConfigureUrls();
        builder.ConfigureCors();

        builder.Services.AddHttpClient("Brevo", client =>
        {
            var url = builder.Configuration["Brevo:BaseUrl"] ?? throw new ApplicationException("Brevo:BaseUrl configuration missing.");
            var key = builder.Configuration["Brevo:ApiKey"] ?? throw new ApplicationException("Brevo:ApiKey configuration missing.");

            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add("api-key", key);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddHttpClient("Gotify", client =>
        {
            var url = builder.Configuration["Gotify:GotifyUrl"] ?? throw new ApplicationException("Gotify:GotifyUrl configuration missing.");
            var key = builder.Configuration["Gotify:GotifyToken"] ?? throw new ApplicationException("Gotify:GotifyToken configuration missing.");

            Console.WriteLine(url);
            Console.WriteLine(key);

            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add("X-Gotify-Key", key);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddHttpClient("Turnstile", client =>
        {
            var url = builder.Configuration["Turnstile:VerifyUrl"] ?? throw new ApplicationException("Turnstile:VerifyUrl configuration missing");
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services
            .AddAuthentication(ApiKeyAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                    ApiKeyAuthenticationHandler.SchemeName, null);

        builder.Services.AddAuthorization();

        builder.ConfigureRateLimiting();

        builder.ConfigureDependencyInjection();

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseRateLimiter();
        app.UseAuthorization();
        app.UseCors();
        app.ConfigureEndpoints();

        app.Run();
    }
}
