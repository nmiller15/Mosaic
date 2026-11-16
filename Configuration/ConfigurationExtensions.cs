using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Mosaic.Auth;
using Mosaic.Features.Turnstile;
using Mosaic.Shared;
using Microsoft.EntityFrameworkCore;

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



    public static void ConfigureApplicationWhitelist(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<List<WhitelistedClient>>(
                builder.Configuration.GetSection("ApplicationWhitelist"));
    }

    public static void ConfigureEmailSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("Zoho")
                );
    }

    public static void ConfigureThingsSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ThingsSettings>(
                builder.Configuration.GetSection("Things")
                );
    }

    public static void ConfigureTurnstileSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<TurnstileSettings>(
                builder.Configuration.GetSection("Turnstile")
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
            if (builder.Environment.IsDevelopment())
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:1313")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            }
            else
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://nolanmiller.me")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            }
        });
    }

    public static void ConfigureRateLimiting(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RateLimitSettings>(
                builder.Configuration.GetSection("RateLimiting"));

        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                var retryAfterSeconds = 60;
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    retryAfterSeconds = (int)retryAfter.TotalSeconds;
                }

                context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds.ToString();

                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    wasSuccessful = false,
                    message = "Rate limit exceeded. Please try again later."
                }, cancellationToken);
            };

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var clientName = context.User?.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
                return RateLimitPartition.GetFixedWindowLimiter(clientName, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = builder.Configuration.GetValue<int>("RateLimiting:PermitLimit", 100),
                    Window = TimeSpan.FromSeconds(
                        builder.Configuration.GetValue<int>("RateLimiting:WindowSeconds", 60)),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });
        });
    }
}
