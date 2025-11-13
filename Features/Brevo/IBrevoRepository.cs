using Mosaic.Shared;

namespace Mosaic.Features.Brevo;

public interface IBrevoRepository
{
    Task<int> AddContact(Contact contact);
}
