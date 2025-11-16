
using Mosaic.Features.Email;

namespace Mosaic.Features.Logging;

public class LoggingService : ILoggingService
{
    private readonly IEmailService _emailService;

    public LoggingService(IEmailRepository emailRepository)
    {
    }

    public Task LogException(Exception ex)
    {
        throw new NotImplementedException();
    }
}
