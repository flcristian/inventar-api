using inventar_api.Locations.Controllers.Interfaces;
using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;
using inventar_api.Locations.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace inventar_api.Locations.Controllers;

public class LocationsController : LocationsApiController
{
    private ILocationsQueryService _queryService;
    private ILocationsCommandService _commandService;
    private ILogger<LocationsController> _logger;

    public LocationsController(ILocationsQueryService queryService, ILocationsCommandService commandService,
        ILogger<LocationsController> logger)
    {
        _queryService = queryService;
        _commandService = commandService;
        _logger = logger;
    }
    
    public override async Task<ActionResult<IEnumerable<Location>>> GetLocations()
    {
        _logger.LogInformation("GET Rest Request: Get all locations.");

        IEnumerable<Location> locations = await _queryService.GetAllLocations();

        return Ok(locations);
    }

    public override async Task<ActionResult<Location>> GetLocationByCode(string code)
    {
        _logger.LogInformation($"GET Rest Request: Get location with code {code}.");

        try
        {
            Location location = await _queryService.GetLocationByCode(code);

            return Ok(location);
        }
        catch (ItemDoesNotExist ex)
        {
            _logger.LogInformation($"404 Rest Response: {ex.Message}");
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<Location>> CreateLocation(CreateLocationRequest request)
    {
        _logger.LogInformation("POST Rest Request: Create location.");

        try
        {
            Location location = await _commandService.CreateLocation(request);

            return Created(ResponseMessages.LOCATION_CREATED, location);
        }
        catch (ItemAlreadyExists ex)
        {
            _logger.LogInformation($"400 Rest Response: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    public override async Task<ActionResult<Location>> DeleteLocation(string code)
    {
        _logger.LogInformation("DELETE Rest Request: Delete location.");

        try
        {
            Location location = await _commandService.DeleteLocation(code);

            return Accepted(ResponseMessages.LOCATION_DELETED, location);
        }
        catch (ItemDoesNotExist ex)
        {
            _logger.LogInformation($"404 Rest Response: {ex.Message}");
            return NotFound(ex.Message);
        }
    }
}