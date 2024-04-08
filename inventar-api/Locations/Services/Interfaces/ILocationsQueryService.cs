using inventar_api.Locations.Models;

namespace inventar_api.Locations.Services.Interfaces;

public interface ILocationsQueryService
{
    Task<IEnumerable<Location>> GetAllLocations();
    Task<Location> GetLocationByCode(string code);
}