using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;

namespace inventar_api.Locations.Services.Interfaces;

public interface ILocationsCommandService
{
    Task<Location> CreateLocation(CreateLocationRequest request);
    Task<Location> DeleteLocation(string code);
}