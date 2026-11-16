using System.Text;
using Mosaic.Features.Email;
using Mosaic.Features.Notifications;

namespace Mosaic.Features.Logging;

public class LoggingService : ILoggingService
{
    private readonly INotificationRepository _notificationRepository;

    public LoggingService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task LogException(Exception ex)
    {
        var builder = new StringBuilder();

        builder.AppendLine(ex.Message + "\n\n");
        foreach (var key in ex.Data.Keys)
        {
            builder.AppendLine($"{key}: {ex.Data[key]}");
        }
        builder.AppendLine("\n");
        builder.AppendLine(ex.StackTrace);

        var notification = new Notification
        {
            Title = ex.Message,
            Message = builder.ToString()
        };

        await _notificationRepository.Send(notification);
    }
}
