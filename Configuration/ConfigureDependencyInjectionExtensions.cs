using Mosaic.Features.Brevo;
using Mosaic.Features.Email;

namespace Mosaic.Configuration;

public static class ConfigureDependencyInjectionExtensions
{
    public static void ConfigureDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.ConfigureRepositories();
        builder.ConfigureServices();
    }

    public static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBrevoRepository, BrevoRepository>();
        builder.Services.AddScoped<IEmailRepository, EmailRepository>();
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBrevoService, BrevoService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
    }
}
