using AutoMapper;
using inventar_api.Data;
using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;
using inventar_api.Locations.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventar_api.Locations.Repository;

public class LocationsRepository : ILocationsRepository
{
    private AppDbContext _context;
    private IMapper _mapper;

    public LocationsRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Location>> GetAllAsync()
    {
        return await _context.Locations
            .Include(l => l.ArticleLocations)
            .ToListAsync();
    }

    public async Task<Location?> GetByCodeAsync(string code)
    {
        return await _context.Locations
            .Include(l => l.ArticleLocations)
            .FirstOrDefaultAsync(l => l.Code.Equals(code));
    }

    public async Task<Location> CreateAsync(CreateLocationRequest request)
    {
        Location location = _mapper.Map<Location>(request);
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
        return location;
    }

    public async Task<Location> DeleteAsync(string code)
    {
        Location location = (await GetByCodeAsync(code))!;
        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();
        return location;
    }
}