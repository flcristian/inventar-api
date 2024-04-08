using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;
using inventar_api.Locations.Repository.Interfaces;

namespace inventar_api.Locations.Services.Interfaces;

public class LocationsCommandService : ILocationsCommandService
{
    private ILocationsRepository _repository;

    public LocationsCommandService(ILocationsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Location> CreateLocation(CreateLocationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Location> DeleteLocation(string code)
    {
        throw new NotImplementedException();
    }
}