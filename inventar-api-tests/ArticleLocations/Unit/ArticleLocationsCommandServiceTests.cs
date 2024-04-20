using inventar_api_tests.ArticleLocations.Helpers;
using inventar_api_tests.Articles.Helpers;
using inventar_api_tests.Locations.Helpers;
using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.ArticleLocations.Models.Comparers;
using inventar_api.ArticleLocations.Repository.Interfaces;
using inventar_api.ArticleLocations.Services;
using inventar_api.ArticleLocations.Services.Interfaces;
using inventar_api.Articles.Models;
using inventar_api.Articles.Repository.Interfaces;
using inventar_api.Locations.Models;
using inventar_api.Locations.Repository.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Moq;

namespace inventar_api_tests.ArticleLocations.Unit;

public class ArticleLocationsCommandServiceTests
{
    private readonly Mock<ILocationsRepository> _mockLocationsRepo;
    private readonly Mock<IArticlesRepository> _mockArticlesRepo;
    private readonly Mock<IArticleLocationsRepository> _mockArticleLocationsRepo;
    private readonly IArticleLocationsCommandService _service;

    public ArticleLocationsCommandServiceTests()
    {
        _mockLocationsRepo = new Mock<ILocationsRepository>();
        _mockArticlesRepo = new Mock<IArticlesRepository>();
        _mockArticleLocationsRepo = new Mock<IArticleLocationsRepository>();
        _service = new ArticleLocationsCommandService(_mockArticleLocationsRepo.Object, _mockArticlesRepo.Object, _mockLocationsRepo.Object);
    }

    [Fact]
    public async Task TestCreateArticleLocation_InvalidArticleCount_ThrowsInvalidValueException()
    {
        CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(1);
        request.Count = -1;
        
        var exception = await Assert.ThrowsAsync<InvalidValue>(() => _service.CreateArticleLocation(request));
        
        Assert.Equal(ExceptionMessages.INVALID_ARTICLE_COUNT, exception.Message);
    }
    
    [Fact]
    public async Task TestCreateArticleLocation_ArticleDoesNotExist_ThrowsItemDoesNotExistException()
    {
        CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(1);
        _mockArticlesRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync((Article)null!);

        var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.CreateArticleLocation(request));
        
        Assert.Equal(ExceptionMessages.ARTICLE_DOES_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestCreateArticleLocation_LocationDoesNotExist_ThrowsItemDoesNotExistException()
    {
        CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(1);
        Article article = TestArticleHelper.CreateArticle(1);
        _mockArticlesRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync(article);
        _mockLocationsRepo.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync((Location)null!);

        var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.CreateArticleLocation(request));
        
        Assert.Equal(ExceptionMessages.LOCATION_DOES_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestCreateArticleLocation_ValidRequest_ReturnsCreatedArticleLocation()
    {
        CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(1);
        Article article = TestArticleHelper.CreateArticle(1);
        Location location = TestLocationHelper.CreateLocation(1);
        ArticleLocation created = TestArticleLocationHelper.CreateArticleLocation(1);
        _mockArticlesRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync(article);
        _mockLocationsRepo.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync(location);
        _mockArticleLocationsRepo.Setup(r => r.CreateAsync(It.IsAny<CreateArticleLocationRequest>()))
            .ReturnsAsync(created);

        var result = await _service.CreateArticleLocation(request);
        
        Assert.Equal(created, result, new ArticleLocationEqualityComparer());
    }

    [Fact]
    public async Task TestUpdateArticleLocation_InvalidArticleCount_ThrowsInvalidValueException()
    {
        UpdateArticleLocationRequest request = TestArticleLocationHelper.CreateUpdateArticleLocationRequest(1);
        request.Count = -1;
        
        var exception = await Assert.ThrowsAsync<InvalidValue>(() => _service.UpdateArticleLocation(request));
        
        Assert.Equal(ExceptionMessages.INVALID_ARTICLE_COUNT, exception.Message);
    }
    
    [Fact]
    public async Task TestUpdateArticleLocation_ArticleLocationDoesNotExist_ThrowsItemDoesNotExistException()
    {
        UpdateArticleLocationRequest request = TestArticleLocationHelper.CreateUpdateArticleLocationRequest(1);
        _mockArticleLocationsRepo.Setup(r => r.GetAsync(It.IsAny<GetArticleLocationRequest>()))
            .ReturnsAsync((ArticleLocation)null!);
        
        var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.UpdateArticleLocation(request));
        
        Assert.Equal(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestUpdateArticleLocation_ValidRequest_ReturnsUpdatedArticleLocation()
    {
        UpdateArticleLocationRequest request = TestArticleLocationHelper.CreateUpdateArticleLocationRequest(1);
        ArticleLocation old = TestArticleLocationHelper.CreateArticleLocation(1);
        ArticleLocation updated = TestArticleLocationHelper.CreateArticleLocation(1);
        _mockArticleLocationsRepo.Setup(r => r.GetAsync(It.IsAny<GetArticleLocationRequest>()))
            .ReturnsAsync(old);
        _mockArticleLocationsRepo.Setup(r => r.UpdateAsync(It.IsAny<UpdateArticleLocationRequest>()))
            .ReturnsAsync(updated);
        
        var result = await _service.UpdateArticleLocation(request);
        
        Assert.Equal(updated, result, new ArticleLocationEqualityComparer());
    }
    
    [Fact]
    public async Task TestDeleteArticleLocation_ArticleLocationDoesNotExist_ThrowsItemDoesNotExistException()
    {
        DeleteArticleLocationRequest request = TestArticleLocationHelper.CreateDeleteArticleLocationRequest(1);
        _mockArticleLocationsRepo.Setup(r => r.GetAsync(It.IsAny<GetArticleLocationRequest>()))
            .ReturnsAsync((ArticleLocation)null!);
        
        var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.DeleteArticleLocation(request));
        
        Assert.Equal(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestDeleteArticleLocation_ValidRequest_ReturnsDeletedArticleLocation()
    {
        DeleteArticleLocationRequest request = TestArticleLocationHelper.CreateDeleteArticleLocationRequest(1);
        ArticleLocation old = TestArticleLocationHelper.CreateArticleLocation(1);
        ArticleLocation deleted = TestArticleLocationHelper.CreateArticleLocation(1);
        _mockArticleLocationsRepo.Setup(r => r.GetAsync(It.IsAny<GetArticleLocationRequest>()))
            .ReturnsAsync(old);
        _mockArticleLocationsRepo.Setup(r => r.DeleteAsync(It.IsAny<DeleteArticleLocationRequest>()))
            .ReturnsAsync(deleted);
        
        var result = await _service.DeleteArticleLocation(request);
        
        Assert.Equal(deleted, result, new ArticleLocationEqualityComparer());
    }
}