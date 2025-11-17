using Mosaic.Features.Brevo;
using Mosaic.Features.Email;
using Mosaic.Features.Logging;

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
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBrevoService, BrevoService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
    }
}
