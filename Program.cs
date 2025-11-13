using Mosaic;
using Mosaic.Configuration;
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

        Console.WriteLine(builder.Configuration["Brevo:BaseUrl"]);
        Console.WriteLine(builder.Configuration["Brevo:ApiKey"].Split('-').First() + "****");

        builder.ConfigureDependencyInjection();

        var app = builder.Build();

        app.UseCors();
        app.ConfigureEndpoints();

        app.Run();
    }
}
