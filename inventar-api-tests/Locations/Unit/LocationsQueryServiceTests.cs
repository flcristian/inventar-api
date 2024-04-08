using inventar_api_tests.Locations.Helpers;
using inventar_api.Locations.Models;
using inventar_api.Locations.Models.Comparers;
using inventar_api.Locations.Repository.Interfaces;
using inventar_api.Locations.Services;
using inventar_api.Locations.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Moq;

namespace inventar_api_tests.Locations.Unit;

public class LocationsQueryServiceTests
{
    private readonly Mock<ILocationsRepository> _mockRepo;
    private readonly ILocationsQueryService _service;

    public LocationsQueryServiceTests()
    {
        _mockRepo = new Mock<ILocationsRepository>();
        _service = new LocationsQueryService(_mockRepo.Object);
    }

    [Fact]
    public async Task TestGetAllLocations_NoLocations_ThrowsItemsDoNotExistException()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Location>());

        var exception = await Assert.ThrowsAsync<ItemsDoNotExist>(() => _service.GetAllLocations());
        
        Assert.Equal(ExceptionMessages.LOCATIONS_DO_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestGetAllLocations_LocationsFound_ReturnsLocationList()
    {
        List<Location> locations = TestLocationHelper.CreateLocations(3);
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(locations);

        var result = await _service.GetAllLocations();
        
        Assert.Equal(3, result.Count());
        Assert.Equal(locations, result, new LocationEqualityComparer());
    }
    
    [Fact]
    public async Task TestGetLocationByCode_LocationNotFound_ThrowsItemDoesNotExistException()
    {
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync((Location)null!);

        var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.GetLocationByCode("1"));
        
        Assert.Equal(ExceptionMessages.LOCATION_DOES_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestGetLocationByCode_LocationFound_ReturnsLocation()
    {
        Location location = TestLocationHelper.CreateLocation(1);
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync(location);

        var result = await _service.GetLocationByCode("1");

        Assert.Equal(location, result, new LocationEqualityComparer());
    }
}