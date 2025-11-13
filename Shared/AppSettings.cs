namespace Mosaic.Shared;

public class AppSettings
{
    public string BrevoApiKey { get; }
    public string BrevoBaseUrl { get; }

    public AppSettings(IConfiguration config)
    {
        BrevoApiKey = config["Brevo:ApiKey"] ?? string.Empty;
        BrevoBaseUrl = config["Brevo:BaseUrl"] ?? string.Empty;
    }
}
