namespace Mosaic.Features.Things;

public interface IThingsRepository
{
    Task<bool> AddToThings(ThingsItem todo);
}
