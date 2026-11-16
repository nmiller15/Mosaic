namespace Mosaic.Features.Notifications;

public interface INotificationRepository
{
    Task Send(Notification notification);
}
