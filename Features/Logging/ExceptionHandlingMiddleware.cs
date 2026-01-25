namespace Mosaic.Features.Logging;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _logger;

    public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILoggingService logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
#if !DEBUG
            await _logger.LogException(ex);
#endif 
            Console.Error.WriteLine($"Exception caught: {ex.Message}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}

