namespace Mosaic.Configuration;

public static class ConfigurationExtensions
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        var basePath = builder.Environment.ContentRootPath;
#if DEBUG
        builder.Configuration.AddJsonFile(
                Path.Combine(basePath, "mosaic.settings.development.json")
                );
#elif RELEASE
        builder.Configuration.AddJsonFile(
                Path.Combine("/", "config", "mosaic.settings.json")
                );
#endif
    }

    public static void ConfigureEmailSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("Email")
                );
    }

    public static void ConfigureUrls(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseUrls(builder.Configuration["ApplicationUrl"]);
    }
}
