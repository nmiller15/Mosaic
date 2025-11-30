using Mosaic.Shared;

namespace Mosaic.Features.Things;

public interface IThingsService
{
    Task<Response<bool>> AddToThings(ThingsItem todo);
}
