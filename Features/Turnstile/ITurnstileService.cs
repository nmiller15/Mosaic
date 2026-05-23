using Mosaic.Shared;

namespace Mosaic.Features.Turnstile;

public interface ITurnstileService
{
    Task<Response<bool>> Verify(string token);
}
