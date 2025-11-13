using Mosaic.Shared;

namespace Mosaic.Features.Brevo;

public interface IBrevoService
{
    Task<Response<string>> AddToNewsletter(string email);
}
