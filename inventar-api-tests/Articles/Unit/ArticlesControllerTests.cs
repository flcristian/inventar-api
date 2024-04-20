using inventar_api_tests.Articles.Helpers;
using inventar_api.Articles.Controllers;
using inventar_api.Articles.Controllers.Interfaces;
using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;
using inventar_api.Articles.Models.Comparers;
using inventar_api.Articles.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace inventar_api_tests.Articles.Unit;

public class ArticlesControllerTests
{
    private readonly Mock<IArticlesQueryService> _mockQueryService;
    private readonly Mock<IArticlesCommandService> _mockCommandService;
    private readonly Mock<ILogger<ArticlesController>> _logger;
    private readonly ArticlesApiController _controller;

    public ArticlesControllerTests()
    {
        _mockQueryService = new Mock<IArticlesQueryService>();
        _mockCommandService = new Mock<IArticlesCommandService>();
        _logger = new Mock<ILogger<ArticlesController>>();
        _controller = new ArticlesController(_mockQueryService.Object, _mockCommandService.Object, _logger.Object);
    }

    [Fact]
    public async Task GetArticles_ArticlesDoNotExist_ReturnsNotFound()
    {
        _mockQueryService.Setup(service => service.GetAllArticles())
            .ThrowsAsync(new ItemsDoNotExist(ExceptionMessages.ARTICLES_DO_NOT_EXIST));

        var result = await _controller.GetArticles();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.ARTICLES_DO_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetArticles_ArticlesExist_ReturnsOkWithArticles()
    {
        var articles = TestArticleHelper.CreateArticles(2);
        _mockQueryService.Setup(service => service.GetAllArticles()).ReturnsAsync(articles);

        var result = await _controller.GetArticles();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedArticles = Assert.IsType<List<Article>>(okResult.Value);
        Assert.Equal(articles, returnedArticles, new ArticleEqualityComparer());
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetArticleByCode_ArticleDoesNotExist_ReturnsNotFound()
    {
        _mockQueryService.Setup(service => service.GetArticleByCode(It.IsAny<int>()))
            .ThrowsAsync(new ItemDoesNotExist(ExceptionMessages.ARTICLE_DOES_NOT_EXIST));

        var result = await _controller.GetArticleByCode(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.ARTICLE_DOES_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetArticleByCode_ArticleExists_ReturnsOkWithArticle()
    {
        var article = TestArticleHelper.CreateArticle(1);
        _mockQueryService.Setup(service => service.GetArticleByCode(It.IsAny<int>())).ReturnsAsync(article);

        var result = await _controller.GetArticleByCode(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedArticle = Assert.IsType<Article>(okResult.Value);
        Assert.Equal(article, returnedArticle, new ArticleEqualityComparer());
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task CreateArticle_InvalidArticleCode_ReturnsBadRequest()
    {
        CreateArticleRequest request = TestArticleHelper.CreateCreateArticleRequest(-1);

        _mockCommandService.Setup(service => service.CreateArticle(It.IsAny<CreateArticleRequest>()))
            .ThrowsAsync(new InvalidValue(ExceptionMessages.INVALID_ARTICLE_CODE));

        var result = await _controller.CreateArticle(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.INVALID_ARTICLE_CODE, badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateArticle_ArticleCodeAlreadyUsed_ReturnsBadRequest()
    {
        CreateArticleRequest request = TestArticleHelper.CreateCreateArticleRequest(1);

        _mockCommandService.Setup(service => service.CreateArticle(It.IsAny<CreateArticleRequest>()))
            .ThrowsAsync(new ItemAlreadyExists(ExceptionMessages.ARTICLE_ALREADY_EXISTS));

        var result = await _controller.CreateArticle(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.ARTICLE_ALREADY_EXISTS, badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateArticle_ValidData_ReturnsOkWithArticle()
    {
        CreateArticleRequest request = TestArticleHelper.CreateCreateArticleRequest(1);
        Article article = TestArticleHelper.CreateArticle(1);

        _mockCommandService.Setup(service => service.CreateArticle(It.IsAny<CreateArticleRequest>()))
            .ReturnsAsync(article);

        var result = await _controller.CreateArticle(request);

        var okResult = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(article, okResult.Value as Article, new ArticleEqualityComparer()!);
        Assert.Equal(201, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateArticle_ArticleDoesNotExist_ReturnsNotFound()
    {
        UpdateArticleRequest request = TestArticleHelper.CreateUpdateArticleRequest(1);
        _mockCommandService.Setup(repo => repo.UpdateArticle(It.IsAny<UpdateArticleRequest>()))
            .ThrowsAsync(new ItemDoesNotExist(ExceptionMessages.ARTICLE_DOES_NOT_EXIST));

        var result = await _controller.UpdateArticle(request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.ARTICLE_DOES_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateArticle_ValidRequest_ReturnsOkWithArticle()
    {
        UpdateArticleRequest request = TestArticleHelper.CreateUpdateArticleRequest(1);
        Article article = TestArticleHelper.CreateArticle(1);
        _mockCommandService.Setup(repo => repo.UpdateArticle(It.IsAny<UpdateArticleRequest>()))
            .ReturnsAsync(article);

        var result = await _controller.UpdateArticle(request);

        var okResult = Assert.IsType<AcceptedResult>(result.Result);
        var returnedArticle = Assert.IsType<Article>(okResult.Value);
        Assert.Equal(article, returnedArticle, new ArticleEqualityComparer());
        Assert.Equal(202, okResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteArticle_ArticleDoesNotExist_ReturnsNotFound()
    {
        _mockCommandService.Setup(repo => repo.DeleteArticle(It.IsAny<int>()))
            .ThrowsAsync(new ItemDoesNotExist(ExceptionMessages.ARTICLE_DOES_NOT_EXIST));

        var result = await _controller.DeleteArticle(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(ExceptionMessages.ARTICLE_DOES_NOT_EXIST, notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteArticle_ArticleExists_ReturnsOkWithArticle()
    {
        Article article = TestArticleHelper.CreateArticle(1);
        
        _mockCommandService.Setup(repo => repo.DeleteArticle(It.IsAny<int>()))
            .ReturnsAsync(article);

        var result = await _controller.DeleteArticle(1);

        var okResult = Assert.IsType<AcceptedResult>(result.Result);
        var returnedArticle = Assert.IsType<Article>(okResult.Value);
        Assert.Equal(article, returnedArticle, new ArticleEqualityComparer());
        Assert.Equal(202, okResult.StatusCode);
    }
}