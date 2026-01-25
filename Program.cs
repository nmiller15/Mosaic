using Microsoft.AspNetCore.Authentication;
using Mosaic;
using Mosaic.Auth;
using Mosaic.Configuration;
using Mosaic.Features.Logging;
using Mosaic.Shared;
using Npgsql;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configure();
        builder.ConfigureApplicationWhitelist();
        builder.ConfigureEmailSettings();
        builder.ConfigureThingsSettings();
        builder.ConfigureUrls();
        builder.ConfigureCors();

        builder.Services.AddHttpClient("Brevo", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Brevo:BaseUrl"]);
            client.DefaultRequestHeaders.Add("api-key", builder.Configuration["Brevo:ApiKey"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services
            .AddAuthentication(ApiKeyAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                    ApiKeyAuthenticationHandler.SchemeName, null);

        builder.Services.AddAuthorization();

        builder.ConfigureDependencyInjection();

        NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors();
        app.ConfigureEndpoints();

        app.Run();
    }
}
