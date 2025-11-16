namespace Mosaic.Features.Logging;

public interface ILoggingService
{
    Task LogException(Exception ex);
}
