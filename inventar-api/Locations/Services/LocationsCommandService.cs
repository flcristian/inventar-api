using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;
using inventar_api.Locations.Repository.Interfaces;
using inventar_api.Locations.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;

namespace inventar_api.Locations.Services;

public class LocationsCommandService : ILocationsCommandService
{
    private ILocationsRepository _repository;

    public LocationsCommandService(ILocationsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Location> CreateLocation(CreateLocationRequest request)
    {
        if (await _repository.GetByCodeAsync(request.Code) != null)
        {
            throw new ItemAlreadyExists(ExceptionMessages.LOCATION_ALREADY_EXISTS);
        }

        return await _repository.CreateAsync(request);
    }

    public async Task<Location> DeleteLocation(string code)
    {
        if (await _repository.GetByCodeAsync(code) == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.LOCATION_DOES_NOT_EXIST);
        }

        return await _repository.DeleteAsync(code);
    }
}