using Mosaic.Features.Logging;
using Mosaic.Shared;

namespace Mosaic.Features.Chipper;

public interface IChipperService
{
    Task<Response<bool>> Log(List<LogMessage> messages);
    Task<Response<bool>> UpdateSubscription(AppSourceSubscriber subscriber);
}
