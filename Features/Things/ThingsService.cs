using Mosaic.Shared;

namespace Mosaic.Features.Things;

public class ThingsService : IThingsService
{
    private readonly IThingsRepository _thingsRepository;

    public ThingsService(IThingsRepository thingsRepository)
    {
        _thingsRepository = thingsRepository;
    }

    public async Task<Response<bool>> AddToThings(ThingsItem todo)
        => Response<bool>.Success(await _thingsRepository.AddToThings(todo));
}
