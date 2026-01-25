using Mosaic.Features.Logging;

namespace Mosaic.Features.Chipper;

public interface IChipperRepository
{
    Task<bool> AddLogMessage(LogMessage message);
    Task<List<AppSourceSubscriber>> GetAppSourceSubscribers(int appSourceKey, string logLevel);
    Task UpdateSubscription(AppSourceSubscriber subscriber);
}
