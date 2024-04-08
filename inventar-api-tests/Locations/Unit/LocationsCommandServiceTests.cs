using inventar_api_tests.Locations.Helpers;
using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;
using inventar_api.Locations.Models.Comparers;
using inventar_api.Locations.Repository.Interfaces;
using inventar_api.Locations.Services;
using inventar_api.Locations.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Moq;

namespace inventar_api_tests.Locations.Unit;

public class LocationsCommandServiceTests
{
    private readonly Mock<ILocationsRepository> _mockRepo;
    private readonly ILocationsCommandService _service;

    public LocationsCommandServiceTests()
    {
        _mockRepo = new Mock<ILocationsRepository>();
        _service = new LocationsCommandService(_mockRepo.Object);
    }

    [Fact]
    public async Task TestCreateLocation_LocationCodeAlreadyUsed_ThrowsItemAlreadyExistsException()
    {
        CreateLocationRequest request = TestLocationHelper.CreateCreateLocationRequest(1);
        Location location = TestLocationHelper.CreateLocation(1);
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync(location);

        var exception = await Assert.ThrowsAsync<ItemAlreadyExists>(() => _service.CreateLocation(request));
        
        Assert.Equal(ExceptionMessages.LOCATION_ALREADY_EXISTS, exception.Message);
    }

    [Fact]
    public async Task TestCreateLocation_ValidRequest_ReturnsCreatedLocation()
    {
        CreateLocationRequest request = TestLocationHelper.CreateCreateLocationRequest(1);
        Location expectedLocation = TestLocationHelper.CreateLocation(1);
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync((Location)null!);
        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<CreateLocationRequest>())).ReturnsAsync(expectedLocation);

        var result = await _service.CreateLocation(request);
        
        Assert.Equal(expectedLocation, result, new LocationEqualityComparer());
    }
    
    [Fact]
    public async Task TestDeleteLocation_LocationNotFound_ThrowsItemDoesNotExistException()
    {
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync((Location)null!);

        var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.DeleteLocation("1"));
        
        Assert.Equal(ExceptionMessages.LOCATION_DOES_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestDeleteLocation_ValidRequest_ReturnsDeletedLocation()
    {
        Location location = TestLocationHelper.CreateLocation(1);
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync(location);
        _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<string>())).ReturnsAsync(location);

        var result = await _service.DeleteLocation("1");
        
        Assert.Equal(location, result, new LocationEqualityComparer());
    }
}