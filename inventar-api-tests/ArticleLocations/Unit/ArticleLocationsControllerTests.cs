using inventar_api_tests.ArticleLocations.Helpers;
using inventar_api.ArticleLocations.Controllers;
using inventar_api.ArticleLocations.Controllers.Interfaces;
using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.ArticleLocations.Models.Comparers;
using inventar_api.ArticleLocations.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace inventar_api_tests.ArticleLocations.Unit;

public class ArticleLocationsControllerTests
{
    private readonly Mock<IArticleLocationsQueryService> _mockQueryService;
    private readonly Mock<IArticleLocationsCommandService> _mockCommandService;
    private readonly Mock<ILogger<ArticleLocationsController>> _logger;
    private readonly ArticleLocationsApiController _controller;

    public ArticleLocationsControllerTests()
    {
        _mockQueryService = new Mock<IArticleLocationsQueryService>();
        _mockCommandService = new Mock<IArticleLocationsCommandService>();
        _logger = new Mock<ILogger<ArticleLocationsController>>();
        _controller = new ArticleLocationsController(_mockQueryService.Object, _mockCommandService.Object, _logger.Object);
    }

    [Fact]
    public async Task GetArticleLocations_ArticleLocationsDoNotExist_ReturnsNotFound()
    {
        _mockQueryService.Setup(service => service.GetAllArticleLocations())
            .ThrowsAsync(new ItemsDoNotExist(ExceptionMessages.ARTICLE_LOCATIONS_DO_NOT_EXIST));

        var result = await _controller.GetArticleLocations();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.ARTICLE_LOCATIONS_DO_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetArticleLocations_ArticleLocationsExist_ReturnsOkWithArticleLocations()
    {
        var articleLocations = TestArticleLocationHelper.CreateArticleLocations(2);
        _mockQueryService.Setup(service => service.GetAllArticleLocations()).ReturnsAsync(articleLocations);

        var result = await _controller.GetArticleLocations();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedArticleLocations = Assert.IsType<List<ArticleLocation>>(okResult.Value);
        Assert.Equal(articleLocations, returnedArticleLocations, new ArticleLocationEqualityComparer());
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetArticleLocationByCode_ArticleLocationDoesNotExist_ReturnsNotFound()
    {
        _mockQueryService.Setup(service => service.GetArticleLocation(It.IsAny<GetArticleLocationRequest>()))
            .ThrowsAsync(new ItemDoesNotExist(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST));
        
        var result = await _controller.GetArticleLocation(1,"1");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetArticleLocationByCode_ArticleLocationExists_ReturnsOkWithArticleLocation()
    {
        var articleLocation = TestArticleLocationHelper.CreateArticleLocation(1);
        _mockQueryService.Setup(service => service.GetArticleLocation(It.IsAny<GetArticleLocationRequest>())).ReturnsAsync(articleLocation);

        var result = await _controller.GetArticleLocation(1, "1");

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedArticleLocation = Assert.IsType<ArticleLocation>(okResult.Value);
        Assert.Equal(articleLocation, returnedArticleLocation, new ArticleLocationEqualityComparer());
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task CreateArticleLocation_InvalidArticleCount_ReturnsBadRequest()
    {
        CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(-1);

        _mockCommandService.Setup(service => service.CreateArticleLocation(It.IsAny<CreateArticleLocationRequest>()))
            .ThrowsAsync(new InvalidValue(ExceptionMessages.INVALID_ARTICLE_COUNT));

        var result = await _controller.CreateArticleLocation(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.INVALID_ARTICLE_COUNT, badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateArticleLocation_ArticleDoesNotExist_ReturnsNotFound()
    {
        CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(1);

        _mockCommandService.Setup(service => service.CreateArticleLocation(It.IsAny<CreateArticleLocationRequest>()))
            .ThrowsAsync(new ItemDoesNotExist(ExceptionMessages.ARTICLE_DOES_NOT_EXIST));

        var result = await _controller.CreateArticleLocation(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.ARTICLE_DOES_NOT_EXIST, badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task CreateArticleLocation_LocationDoesNotExist_ReturnsNotFound()
    {
        CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(1);

        _mockCommandService.Setup(service => service.CreateArticleLocation(It.IsAny<CreateArticleLocationRequest>()))
            .ThrowsAsync(new ItemDoesNotExist(ExceptionMessages.LOCATION_DOES_NOT_EXIST));

        var result = await _controller.CreateArticleLocation(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.LOCATION_DOES_NOT_EXIST, badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateArticleLocation_ValidData_ReturnsOkWithArticleLocation()
    {
        CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(1);
        ArticleLocation articleLocation = TestArticleLocationHelper.CreateArticleLocation(1);

        _mockCommandService.Setup(service => service.CreateArticleLocation(It.IsAny<CreateArticleLocationRequest>()))
            .ReturnsAsync(articleLocation);

        var result = await _controller.CreateArticleLocation(request);

        var okResult = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(articleLocation, okResult.Value as ArticleLocation, new ArticleLocationEqualityComparer()!);
        Assert.Equal(201, okResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateArticleLocation_InvalidArticleCount_ReturnsBadRequest()
    {
        UpdateArticleLocationRequest request = TestArticleLocationHelper.CreateUpdateArticleLocationRequest(-1);
        _mockCommandService.Setup(service => service.UpdateArticleLocation(It.IsAny<UpdateArticleLocationRequest>()))
            .ThrowsAsync(new InvalidValue(ExceptionMessages.INVALID_ARTICLE_COUNT));

        var result = await _controller.UpdateArticleLocation(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.INVALID_ARTICLE_COUNT, badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpdateArticleLocation_ArticleLocationDoesNotExist_ReturnsNotFound()
    {
        UpdateArticleLocationRequest request = TestArticleLocationHelper.CreateUpdateArticleLocationRequest(1);
        _mockCommandService.Setup(repo => repo.UpdateArticleLocation(It.IsAny<UpdateArticleLocationRequest>()))
            .ThrowsAsync(new ItemDoesNotExist(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST));

        var result = await _controller.UpdateArticleLocation(request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateArticleLocation_ValidRequest_ReturnsOkWithArticleLocation()
    {
        UpdateArticleLocationRequest request = TestArticleLocationHelper.CreateUpdateArticleLocationRequest(1);
        ArticleLocation articleLocation = TestArticleLocationHelper.CreateArticleLocation(1);
        _mockCommandService.Setup(repo => repo.UpdateArticleLocation(It.IsAny<UpdateArticleLocationRequest>()))
            .ReturnsAsync(articleLocation);

        var result = await _controller.UpdateArticleLocation(request);

        var okResult = Assert.IsType<AcceptedResult>(result.Result);
        var returnedArticleLocation = Assert.IsType<ArticleLocation>(okResult.Value);
        Assert.Equal(articleLocation, returnedArticleLocation, new ArticleLocationEqualityComparer());
        Assert.Equal(202, okResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteArticleLocation_ArticleLocationDoesNotExist_ReturnsNotFound()
    {
        _mockCommandService.Setup(repo => repo.DeleteArticleLocation(It.IsAny<DeleteArticleLocationRequest>()))
            .ThrowsAsync(new ItemDoesNotExist(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST));

        var result = await _controller.DeleteArticleLocation(1, "1");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteArticleLocation_ArticleLocationExists_ReturnsOkWithArticleLocation()
    {
        ArticleLocation articleLocation = TestArticleLocationHelper.CreateArticleLocation(1);
        
        _mockCommandService.Setup(repo => repo.DeleteArticleLocation(It.IsAny<DeleteArticleLocationRequest>()))
            .ReturnsAsync(articleLocation);

        var result = await _controller.DeleteArticleLocation(1, "1");

        var okResult = Assert.IsType<AcceptedResult>(result.Result);
        var returnedArticleLocation = Assert.IsType<ArticleLocation>(okResult.Value);
        Assert.Equal(articleLocation, returnedArticleLocation, new ArticleLocationEqualityComparer());
        Assert.Equal(202, okResult.StatusCode);
    }
}