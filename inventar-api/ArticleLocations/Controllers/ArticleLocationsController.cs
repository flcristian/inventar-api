using inventar_api.ArticleLocations.Controllers.Interfaces;
using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.ArticleLocations.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace inventar_api.ArticleLocations.Controllers;

public class ArticleLocationsController : ArticleLocationsApiController
{
    private IArticleLocationsQueryService _queryService;
    private IArticleLocationsCommandService _commandService;
    private ILogger<ArticleLocationsController> _logger;

    public ArticleLocationsController(IArticleLocationsQueryService queryService, IArticleLocationsCommandService commandService,
        ILogger<ArticleLocationsController> logger)
    {
        _queryService = queryService;
        _commandService = commandService;
        _logger = logger;
    }
    
    public override async Task<ActionResult<IEnumerable<ArticleLocation>>> GetArticleLocations()
    {
        _logger.LogInformation("GET Rest Request: Get all article locations.");

        try
        {
            IEnumerable<ArticleLocation> articleLocations = await _queryService.GetAllArticleLocations();

            return Ok(articleLocations);
        }
        catch (ItemsDoNotExist ex)
        {
            _logger.LogInformation($"404 Rest Response: {ex.Message}");
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<ArticleLocation>> GetArticleLocation(int articleCode, string locationCode)
    {
        _logger.LogInformation($"GET Rest Request: Get article location with article code {articleCode} and location code {locationCode}.");

        try
        {
            GetArticleLocationRequest request = new GetArticleLocationRequest
            {
                ArticleCode = articleCode,
                LocationCode = locationCode
            };
            
            ArticleLocation articleLocation = await _queryService.GetArticleLocation(request);

            return Ok(articleLocation);
        }
        catch (ItemDoesNotExist ex)
        {
            _logger.LogInformation($"404 Rest Response: {ex.Message}");
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<ArticleLocation>> CreateArticleLocation(CreateArticleLocationRequest request)
    {
        _logger.LogInformation("POST Rest Request: Create article location.");

        try
        {
            ArticleLocation articleLocation = await _commandService.CreateArticleLocation(request);

            return Created(ResponseMessages.ARTICLE_LOCATION_CREATED, articleLocation);
        }
        catch (InvalidValue ex)
        {
            _logger.LogInformation($"400 Rest Response: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (ItemDoesNotExist ex)
        {
            _logger.LogInformation($"404 Rest Response: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    public override async Task<ActionResult<ArticleLocation>> UpdateArticleLocation(UpdateArticleLocationRequest request)
    {
        _logger.LogInformation("PUT Rest Request: Update article location.");

        try
        {
            ArticleLocation articleLocation = await _commandService.UpdateArticleLocation(request);

            return Accepted(ResponseMessages.ARTICLE_LOCATION_UPDATED, articleLocation);
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

    public override async Task<ActionResult<ArticleLocation>> DeleteArticleLocation(int articleCode, string locationCode)
    {
        _logger.LogInformation("DELETE Rest Request: Delete article location.");

        try
        {
            DeleteArticleLocationRequest request = new DeleteArticleLocationRequest
            {
                ArticleCode = articleCode,
                LocationCode = locationCode
            };
            
            ArticleLocation articleLocation = await _commandService.DeleteArticleLocation(request);

            return Accepted(ResponseMessages.ARTICLE_LOCATION_DELETED, articleLocation);
        }
        catch (ItemDoesNotExist ex)
        {
            _logger.LogInformation($"404 Rest Response: {ex.Message}");
            return NotFound(ex.Message);
        }
    }
}