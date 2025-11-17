using System.Text;
using Mosaic.Features.Email;

namespace Mosaic.Features.Logging;

public class LoggingService : ILoggingService
{
    private readonly IEmailRepository _emailRepository;

    public LoggingService(IEmailRepository emailRepository)
    {
        _emailRepository = emailRepository;
    }

    public async Task LogException(Exception ex)
    {
        var message = $"Mosaic API | {ex.Message}";
        var builder = new StringBuilder();

        builder.Append($"<h1>Mosaic API | {ex.Message}</h1>");
        builder.Append($"<p>{ex.Message}</p>");

        builder.Append("<h2>Data</h2>");
        builder.Append("<ul>");
        foreach (var key in ex.Data.Keys)
        {
            builder.Append($"<li>{key}: {ex.Data[key]}</li>");
        }
        builder.Append("</ul>");

        builder.Append("<h2>Stack Trace</h2>");
        builder.Append($"<pre>{ex.StackTrace}</pre>");

        await _emailRepository.SendNotificationToSubscribers(message, builder.ToString());
    }
}
