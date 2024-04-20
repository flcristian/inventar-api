using inventar_api_tests.Locations.Helpers;
using inventar_api.Locations.Controllers;
using inventar_api.Locations.Controllers.Interfaces;
using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;
using inventar_api.Locations.Models.Comparers;
using inventar_api.Locations.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace inventar_api_tests.Locations.Unit;

public class LocationsControllerTests
{
    private readonly Mock<ILocationsQueryService> _mockQueryService;
    private readonly Mock<ILocationsCommandService> _mockCommandService;
    private readonly Mock<ILogger<LocationsController>> _logger;
    private readonly LocationsApiController _controller;

    public LocationsControllerTests()
    {
        _mockQueryService = new Mock<ILocationsQueryService>();
        _mockCommandService = new Mock<ILocationsCommandService>();
        _logger = new Mock<ILogger<LocationsController>>();
        _controller = new LocationsController(_mockQueryService.Object, _mockCommandService.Object, _logger.Object);
    }

    [Fact]
    public async Task GetLocations_LocationsDoNotExist_ReturnsNotFound()
    {
        _mockQueryService.Setup(service => service.GetAllLocations())
            .ThrowsAsync(new ItemsDoNotExist(ExceptionMessages.LOCATIONS_DO_NOT_EXIST));

        var result = await _controller.GetLocations();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.LOCATIONS_DO_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetLocations_LocationsExist_ReturnsOkWithLocations()
    {
        var locations = TestLocationHelper.CreateLocations(2);
        _mockQueryService.Setup(service => service.GetAllLocations()).ReturnsAsync(locations);

        var result = await _controller.GetLocations();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedLocations = Assert.IsType<List<Location>>(okResult.Value);
        Assert.Equal(locations, returnedLocations, new LocationEqualityComparer());
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetLocationByCode_LocationDoesNotExist_ReturnsNotFound()
    {
        _mockQueryService.Setup(service => service.GetLocationByCode(It.IsAny<string>()))
            .ThrowsAsync(new ItemDoesNotExist(ExceptionMessages.LOCATION_DOES_NOT_EXIST));

        var result = await _controller.GetLocationByCode("1");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.LOCATION_DOES_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetLocationByCode_LocationExists_ReturnsOkWithLocation()
    {
        var location = TestLocationHelper.CreateLocation(1);
        _mockQueryService.Setup(service => service.GetLocationByCode(It.IsAny<string>())).ReturnsAsync(location);

        var result = await _controller.GetLocationByCode("1");

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedLocation = Assert.IsType<Location>(okResult.Value);
        Assert.Equal(location, returnedLocation, new LocationEqualityComparer());
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CreateLocation_LocationCodeAlreadyUsed_ReturnsBadRequest()
    {
        CreateLocationRequest request = TestLocationHelper.CreateCreateLocationRequest(1);

        _mockCommandService.Setup(service => service.CreateLocation(It.IsAny<CreateLocationRequest>()))
            .ThrowsAsync(new ItemAlreadyExists(ExceptionMessages.LOCATION_ALREADY_EXISTS));

        var result = await _controller.CreateLocation(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.LOCATION_ALREADY_EXISTS, badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateLocation_ValidData_ReturnsOkWithLocation()
    {
        CreateLocationRequest request = TestLocationHelper.CreateCreateLocationRequest(1);
        Location location = TestLocationHelper.CreateLocation(1);

        _mockCommandService.Setup(service => service.CreateLocation(It.IsAny<CreateLocationRequest>()))
            .ReturnsAsync(location);

        var result = await _controller.CreateLocation(request);

        var okResult = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(location, okResult.Value as Location, new LocationEqualityComparer()!);
        Assert.Equal(201, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteLocation_LocationDoesNotExist_ReturnsNotFound()
    {
        _mockCommandService.Setup(repo => repo.DeleteLocation(It.IsAny<string>()))
            .ThrowsAsync(new ItemDoesNotExist(ExceptionMessages.LOCATION_DOES_NOT_EXIST));

        var result = await _controller.DeleteLocation("1");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.LOCATION_DOES_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteLocation_LocationExists_ReturnsOkWithLocation()
    {
        Location location = TestLocationHelper.CreateLocation(1);
        
        _mockCommandService.Setup(repo => repo.DeleteLocation(It.IsAny<string>()))
            .ReturnsAsync(location);

        var result = await _controller.DeleteLocation("1");

        var notFoundResult = Assert.IsType<AcceptedResult>(result.Result);
        Assert.Equal(location, notFoundResult.Value as Location, new LocationEqualityComparer()!);
        Assert.Equal(202, notFoundResult.StatusCode);
    }
}