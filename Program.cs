using Mosaic;
using Mosaic.Configuration;
using Mosaic.Shared;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configure();
        builder.ConfigureEmailSettings();

        builder.Services.AddHttpClient("Brevo", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Brevo:BaseUrl"]);
            client.DefaultRequestHeaders.Add("api-key", builder.Configuration["Brevo:ApiKey"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.ConfigureDependencyInjection();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.ConfigureEndpoints();

        app.Run();
    }
}
