using Mosaic.Features.Brevo;
using Mosaic.Features.Email;
using Mosaic.Features.Logging;
using Mosaic.Features.Things;

namespace Mosaic.Configuration;

public static class ConfigureDependencyInjectionExtensions
{
    public static void ConfigureDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.ConfigureRoots();
        builder.ConfigureRepositories();
        builder.ConfigureServices();
    }

    public static void ConfigureRoots(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IEmailRepository, EmailRepository>();
        builder.Services.AddSingleton<ILoggingService, LoggingService>();
    }

    public static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBrevoRepository, BrevoRepository>();
        builder.Services.AddScoped<IThingsRepository, ThingsRepository>();
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBrevoService, BrevoService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IThingsService, ThingsService>();
    }
}
