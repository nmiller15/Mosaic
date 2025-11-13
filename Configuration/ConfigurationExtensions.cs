namespace Mosaic.Configuration;

public static class ConfigurationExtensions
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        var basePath = builder.Environment.ContentRootPath;

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddJsonFile(
                    Path.Combine(basePath, "mosaic.settings.development.json")
                    );
        }
        else
        {
            builder.Configuration.AddJsonFile(
                    Path.Combine("/", "config", "mosaic.settings.json")
                    );
        }
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

    public static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                    {
                        policy.WithOrigins("https://nolanmiller.me")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
        });
    }
}
