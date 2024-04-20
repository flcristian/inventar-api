using System.Net;
using System.Text;
using inventar_api_tests.Infrastructure;
using inventar_api_tests.Locations.Helpers;
using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;
using inventar_api.Locations.Models.Comparers;
using Newtonsoft.Json;

namespace inventar_api_tests.Locations.Integration;

public class LocationsIntegrationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public LocationsIntegrationTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Get_GetLocations_NoLocationsExist_ReturnsNotFoundStatusCode()
    {
        var request = "/api/v1/Locations/all";
        
        var response = await _client.GetAsync(request);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Get_GetLocations_LocationsFound_ReturnsOkStatusCode_ValidLocationsContentResponse()
    {
        var createLocationRequest = "/api/v1/Locations/create";
        CreateLocationRequest locationRequest1 = TestLocationHelper.CreateCreateLocationRequest(1);
        CreateLocationRequest locationRequest2 = TestLocationHelper.CreateCreateLocationRequest(2);
        var content1 = new StringContent(JsonConvert.SerializeObject(locationRequest1), Encoding.UTF8, "application/json");
        var content2 = new StringContent(JsonConvert.SerializeObject(locationRequest2), Encoding.UTF8, "application/json");
        await _client.PostAsync(createLocationRequest, content1);
        await _client.PostAsync(createLocationRequest, content2);
        
        var request = "/api/v1/Locations/all";
        var response = await _client.GetAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<Location>>(responseString);
        var locations = TestLocationHelper.CreateLocations(2);
        Assert.Equal(locations, result, new LocationEqualityComparer());
        
        var deleteRequest = "/api/v1/Locations/delete/";
        await _client.DeleteAsync(deleteRequest + "1");
        await _client.DeleteAsync(deleteRequest + "2");
    }

    [Fact]
    public async Task Get_GetLocationByCode_LocationNotFound_ReturnsNotFoundStatusCode()
    {
        var request = "/api/v1/Locations/location/3";
        
        var response = await _client.GetAsync(request);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_GetLocationByCode_LocationFound_ReturnsOkStatusCode_ValidLocationContentResponse()
    {
        var createLocationRequest = "/api/v1/Locations/create";
        CreateLocationRequest locationRequest = TestLocationHelper.CreateCreateLocationRequest(4);
        var content = new StringContent(JsonConvert.SerializeObject(locationRequest), Encoding.UTF8, "application/json");
        await _client.PostAsync(createLocationRequest, content);
        
        var request = "/api/v1/Locations/location/4";
        var response = await _client.GetAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Location>(responseString);
        var location = TestLocationHelper.CreateLocation(4);
        Assert.Equal(location.Code, result.Code);
        
        var deleteRequest = "/api/v1/Locations/delete/";
        await _client.DeleteAsync(deleteRequest + "4");
    }

    [Fact]
    public async Task Post_CreateLocation_CodeAlreadyUsed_ReturnsBadRequestStatusCode()
    {
        var createRequest = "/api/v1/Locations/create";
        CreateLocationRequest request = TestLocationHelper.CreateCreateLocationRequest(5);
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        await _client.PostAsync(createRequest, content);
        
        var response = await _client.PostAsync(createRequest, content);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var deleteRequest = "/api/v1/Locations/delete/";
        await _client.DeleteAsync(deleteRequest + "5");
    }
    
    [Fact]
    public async Task Post_CreateLocation_ValidRequest_ReturnsCreatedStatusCode_ValidLocationContentResponse()
    {
        var createRequest = "/api/v1/Locations/create";
        CreateLocationRequest request = TestLocationHelper.CreateCreateLocationRequest(6);
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync(createRequest, content);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Location>(responseString);
        var location = TestLocationHelper.CreateLocation(6);
        Assert.Equal(location.Code, result.Code);
                
        var deleteRequest = "/api/v1/Locations/delete/";
        await _client.DeleteAsync(deleteRequest + "6");
    }

    [Fact]
    public async Task Delete_DeleteLocation_LocationNotFound_ReturnsNotFoundStatusCode()
    {
        var deleteRequest = "/api/v1/Locations/delete/";
        
        var response = await _client.DeleteAsync(deleteRequest + "7");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}