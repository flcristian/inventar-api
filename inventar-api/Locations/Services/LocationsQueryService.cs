using inventar_api.Locations.Models;
using inventar_api.Locations.Repository.Interfaces;
using inventar_api.Locations.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;

namespace inventar_api.Locations.Services;

public class LocationsQueryService : ILocationsQueryService
{
    private ILocationsRepository _repository;

    public LocationsQueryService(ILocationsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Location>> GetAllLocations()
    {
        IEnumerable<Location> result = await _repository.GetAllAsync();

        if (result.Count() == 0)
        {
            throw new ItemsDoNotExist(ExceptionMessages.LOCATIONS_DO_NOT_EXIST);
        }

        return result;
    }

    public async Task<Location> GetLocationByCode(string code)
    {
        Location? result = await _repository.GetByCodeAsync(code);

        if (result == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.LOCATION_DOES_NOT_EXIST);
        }

        return result;
    }
}