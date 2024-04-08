using inventar_api.Locations.Models;
using inventar_api.Locations.Repository.Interfaces;
using inventar_api.Locations.Services.Interfaces;

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
        throw new NotImplementedException();
    }

    public async Task<Location> GetLocationByCode(string code)
    {
        throw new NotImplementedException();
    }
}