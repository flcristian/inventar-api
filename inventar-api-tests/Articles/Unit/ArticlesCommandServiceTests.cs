using inventar_api_tests.Articles.Helpers;
using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;
using inventar_api.Articles.Models.Comparers;
using inventar_api.Articles.Repository.Interfaces;
using inventar_api.Articles.Services;
using inventar_api.Articles.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Moq;

namespace inventar_api_tests.Articles.Unit;

public class ArticlesCommandServiceTests
{
    private readonly Mock<IArticlesRepository> _mockRepo;
    private readonly IArticlesCommandService _service;

    public ArticlesCommandServiceTests()
    {
        _mockRepo = new Mock<IArticlesRepository>();
        _service = new ArticlesCommandService(_mockRepo.Object);
    }

    [Fact]
    public async Task TestCreateArticle_InvalidCode_ThrowsInvalidValueException()
    {
        CreateArticleRequest request = TestArticleHelper.CreateCreateArticleRequest(-1);

        var exception = await Assert.ThrowsAsync<InvalidValue>(() => _service.CreateArticle(request));
        
        Assert.Equal(ExceptionMessages.INVALID_ARTICLE_CODE, exception.Message);
    }

    [Fact]
    public async Task TestCreateArticle_ArticleCodeAlreadyUsed_ThrowsItemAlreadyExistsException()
    {
        CreateArticleRequest request = TestArticleHelper.CreateCreateArticleRequest(1);
        Article article = TestArticleHelper.CreateArticle(1);
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync(article);

        var exception = await Assert.ThrowsAsync<ItemAlreadyExists>(() => _service.CreateArticle(request));
        
        Assert.Equal(ExceptionMessages.ARTICLE_ALREADY_EXISTS, exception.Message);
    }

    [Fact]
    public async Task TestCreateArticle_ValidRequest_ReturnsCreatedArticle()
    {
        CreateArticleRequest request = TestArticleHelper.CreateCreateArticleRequest(1);
        Article expectedArticle = TestArticleHelper.CreateArticle(1);
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync((Article)null!);
        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<CreateArticleRequest>())).ReturnsAsync(expectedArticle);

        var result = await _service.CreateArticle(request);
        
        Assert.Equal(expectedArticle, result, new ArticleEqualityComparer());
    }

    [Fact]
    public async Task TestUpdateArticle_InvalidCode_ThrowsInvalidValueException()
    {
        UpdateArticleRequest request = TestArticleHelper.CreateUpdateArticleRequest(-1);

        var exception = await Assert.ThrowsAsync<InvalidValue>(() => _service.UpdateArticle(request));
        
        Assert.Equal(ExceptionMessages.INVALID_ARTICLE_CODE, exception.Message);
    }

    [Fact]
    public async Task TestUpdateArticle_ArticleNotFound_ThrowsItemDoesNotExistException()
    {
        UpdateArticleRequest request = TestArticleHelper.CreateUpdateArticleRequest(1);
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync((Article)null!);

        var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.UpdateArticle(request));
        
        Assert.Equal(ExceptionMessages.ARTICLE_DOES_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestUpdateArticle_ValidRequest_ReturnsUpdatedArticle()
    {
        UpdateArticleRequest request = TestArticleHelper.CreateUpdateArticleRequest(1);
        Article article = TestArticleHelper.CreateArticle(1);
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync(article);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<UpdateArticleRequest>())).ReturnsAsync(article);

        var result = await _service.UpdateArticle(request);
        
        Assert.Equal(article, result, new ArticleEqualityComparer());
    }
    
    [Fact]
    public async Task TestDeleteArticle_ArticleNotFound_ThrowsItemDoesNotExistException()
    {
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync((Article)null!);

        var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.DeleteArticle(1));
        
        Assert.Equal(ExceptionMessages.ARTICLE_DOES_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestDeleteArticle_ValidRequest_ReturnsDeletedArticle()
    {
        Article article = TestArticleHelper.CreateArticle(1);
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync(article);
        _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<int>())).ReturnsAsync(article);

        var result = await _service.DeleteArticle(1);
        
        Assert.Equal(article, result, new ArticleEqualityComparer());
    }
}