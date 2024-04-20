using inventar_api.Articles.Controllers.Interfaces;
using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;
using inventar_api.Articles.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace inventar_api.Articles.Controllers;

public class ArticlesController : ArticlesApiController
{
    private IArticlesQueryService _queryService;
    private IArticlesCommandService _commandService;
    private ILogger<ArticlesController> _logger;

    public ArticlesController(IArticlesQueryService queryService, IArticlesCommandService commandService,
        ILogger<ArticlesController> logger)
    {
        _queryService = queryService;
        _commandService = commandService;
        _logger = logger;
    }
    
    public override async Task<ActionResult<IEnumerable<Article>>> GetArticles()
    {
        _logger.LogInformation("GET Rest Request: Get all articles.");

        try
        {
            IEnumerable<Article> articles = await _queryService.GetAllArticles();

            return Ok(articles);
        }
        catch (ItemsDoNotExist ex)
        {
            _logger.LogInformation($"404 Rest Response: {ex.Message}");
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<Article>> GetArticleByCode(int code)
    {
        _logger.LogInformation($"GET Rest Request: Get article with code {code}.");

        try
        {
            Article article = await _queryService.GetArticleByCode(code);

            return Ok(article);
        }
        catch (ItemDoesNotExist ex)
        {
            _logger.LogInformation($"404 Rest Response: {ex.Message}");
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<Article>> CreateArticle(CreateArticleRequest request)
    {
        _logger.LogInformation("POST Rest Request: Create article.");

        try
        {
            Article article = await _commandService.CreateArticle(request);

            return Created(ResponseMessages.ARTICLE_CREATED, article);
        }
        catch (InvalidValue ex)
        {
            _logger.LogInformation($"400 Rest Response: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (ItemAlreadyExists ex)
        {
            _logger.LogInformation($"400 Rest Response: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    public override async Task<ActionResult<Article>> UpdateArticle(UpdateArticleRequest request)
    {
        _logger.LogInformation("PUT Rest Request: Update article.");

        try
        {
            Article article = await _commandService.UpdateArticle(request);

            return Accepted(ResponseMessages.ARTICLE_UPDATED, article);
        }
        catch (InvalidValue ex)
        {
            _logger.LogInformation($"400 Rest Response: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (ItemDoesNotExist ex)
        {
            _logger.LogInformation($"404 Rest Response: {ex.Message}");
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<Article>> DeleteArticle(int code)
    {
        _logger.LogInformation("DELETE Rest Request: Delete article.");

        try
        {
            Article article = await _commandService.DeleteArticle(code);

            return Accepted(ResponseMessages.ARTICLE_DELETED, article);
        }
        catch (ItemDoesNotExist ex)
        {
            _logger.LogInformation($"404 Rest Response: {ex.Message}");
            return NotFound(ex.Message);
        }
    }
}