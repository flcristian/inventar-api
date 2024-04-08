using inventar_api_tests.ArticleLocations.Helpers;
using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.ArticleLocations.Models.Comparers;
using inventar_api.ArticleLocations.Repository.Interfaces;
using inventar_api.ArticleLocations.Services;
using inventar_api.ArticleLocations.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Moq;

namespace inventar_api_tests.ArticleLocations.Unit;

public class ArticleLocationsQueryServiceTests
{
    private readonly Mock<IArticleLocationsRepository> _mockRepo;
    private readonly IArticleLocationsQueryService _service;

    public ArticleLocationsQueryServiceTests()
    {
        _mockRepo = new Mock<IArticleLocationsRepository>();
        _service = new ArticleLocationsQueryService(_mockRepo.Object);
    }

    [Fact]
    public async Task TestGetAllArticleLocations_NoArticleLocations_ThrowsItemsDoNotExistException()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ArticleLocation>());

        var exception = await Assert.ThrowsAsync<ItemsDoNotExist>(() => _service.GetAllArticleLocations());
        
        Assert.Equal(ExceptionMessages.ARTICLE_LOCATIONS_DO_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestGetAllArticleLocations_ArticleLocationsFound_ReturnsArticleLocationList()
    {
        List<ArticleLocation> articleLocations = TestArticleLocationHelper.CreateArticleLocations(3);
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(articleLocations);

        var result = await _service.GetAllArticleLocations();
        
        Assert.Equal(3, result.Count());
        Assert.Equal(articleLocations, result, new ArticleLocationEqualityComparer());
    }
    
    [Fact]
    public async Task TestGetArticleLocationByCode_ArticleLocationNotFound_ThrowsItemDoesNotExistException()
    {
        GetArticleLocationRequest request = TestArticleLocationHelper.CreateGetArticleLocationRequest(1);
        _mockRepo.Setup(r => r.GetAsync(It.IsAny<GetArticleLocationRequest>())).ReturnsAsync((ArticleLocation)null!);

        var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.GetArticleLocation(request));
        
        Assert.Equal(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestGetArticleLocationByCode_ArticleLocationFound_ReturnsArticleLocation()
    {
        GetArticleLocationRequest request = TestArticleLocationHelper.CreateGetArticleLocationRequest(1);
        ArticleLocation articleLocation = TestArticleLocationHelper.CreateArticleLocation(1);
        _mockRepo.Setup(r => r.GetAsync(It.IsAny<GetArticleLocationRequest>())).ReturnsAsync(articleLocation);

        var result = await _service.GetArticleLocation(request);

        Assert.Equal(articleLocation, result, new ArticleLocationEqualityComparer());
    }
}