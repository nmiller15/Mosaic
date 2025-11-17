using Mosaic;
using Mosaic.Configuration;
using Mosaic.Features.Logging;
using Mosaic.Shared;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configure();
        builder.ConfigureEmailSettings();
        builder.ConfigureUrls();
        builder.ConfigureCors();

        builder.Services.AddHttpClient("Brevo", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Brevo:BaseUrl"]);
            client.DefaultRequestHeaders.Add("api-key", builder.Configuration["Brevo:ApiKey"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.ConfigureDependencyInjection();

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseCors();
        app.ConfigureEndpoints();

        app.Run();
    }
}
