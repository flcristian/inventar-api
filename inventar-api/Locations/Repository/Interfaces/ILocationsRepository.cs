using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;

namespace inventar_api.Locations.Repository.Interfaces;

public interface ILocationsRepository
{
    Task<IEnumerable<Location>> GetAllAsync();
    Task<Location?> GetByCodeAsync(string code);
    Task<Location> CreateAsync(CreateLocationRequest request);
    Task<Location> DeleteAsync(string code);
}