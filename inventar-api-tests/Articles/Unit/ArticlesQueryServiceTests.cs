using inventar_api_tests.Articles.Helpers;
using inventar_api.Articles.Models;
using inventar_api.Articles.Models.Comparers;
using inventar_api.Articles.Repository.Interfaces;
using inventar_api.Articles.Services;
using inventar_api.Articles.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Moq;

namespace inventar_api_tests.Articles.Unit;

public class ArticlesQueryServiceTests
{
    private readonly Mock<IArticlesRepository> _mockRepo;
    private readonly IArticlesQueryService _service;

    public ArticlesQueryServiceTests()
    {
        _mockRepo = new Mock<IArticlesRepository>();
        _service = new ArticlesQueryService(_mockRepo.Object);
    }

    [Fact]
    public async Task TestGetAllArticles_NoArticles_ThrowsItemsDoNotExistException()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Article>());

        var exception = await Assert.ThrowsAsync<ItemsDoNotExist>(() => _service.GetAllArticles());
        
        Assert.Equal(ExceptionMessages.ARTICLES_DO_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestGetAllArticles_ArticlesFound_ReturnsArticleList()
    {
        List<Article> articles = TestArticleHelper.CreateArticles(3);
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(articles);

        var result = await _service.GetAllArticles();
        
        Assert.Equal(3, result.Count());
        Assert.Equal(articles, result, new ArticleEqualityComparer());
    }
    
    [Fact]
    public async Task TestGetArticleByCode_ArticleNotFound_ThrowsItemDoesNotExistException()
    {
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync((Article)null!);

        var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.GetArticleByCode(1));
        
        Assert.Equal(ExceptionMessages.ARTICLE_DOES_NOT_EXIST, exception.Message);
    }
    
    [Fact]
    public async Task TestGetArticleByCode_ArticleFound_ReturnsArticle()
    {
        Article article = TestArticleHelper.CreateArticle(1);
        _mockRepo.Setup(r => r.GetByCodeAsync(It.IsAny<int>())).ReturnsAsync(article);

        var result = await _service.GetArticleByCode(1);

        Assert.Equal(article, result, new ArticleEqualityComparer());
    }
}