using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.ArticleLocations.Repository.Interfaces;
using inventar_api.ArticleLocations.Services.Interfaces;
using inventar_api.Articles.Models;
using inventar_api.Articles.Repository.Interfaces;
using inventar_api.Locations.Models;
using inventar_api.Locations.Repository.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;

namespace inventar_api.ArticleLocations.Services;

public class ArticleLocationsCommandService : IArticleLocationsCommandService
{
    private IArticleLocationsRepository _articleLocationsRepository;
    private IArticlesRepository _articlesRepository;
    private ILocationsRepository _locationsRepository;

    public ArticleLocationsCommandService(IArticleLocationsRepository articleLocationsRepository, IArticlesRepository articlesRepository, ILocationsRepository locationsRepository)
    {
        _articleLocationsRepository = articleLocationsRepository;
        _articlesRepository = articlesRepository;
        _locationsRepository = locationsRepository;
    }

    public async Task<ArticleLocation> CreateArticleLocation(CreateArticleLocationRequest request)
    {
        if (request.Count < 0)
        {
            throw new InvalidValue(ExceptionMessages.INVALID_ARTICLE_COUNT);
        }
        
        if (await _articlesRepository.GetByCodeAsync(request.ArticleCode) == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.ARTICLE_DOES_NOT_EXIST);
        }
        
        if (await _locationsRepository.GetByCodeAsync(request.LocationCode) == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.LOCATION_DOES_NOT_EXIST);
        }

        ArticleLocation result = await _articleLocationsRepository.CreateAsync(request);
        return result;
    }

    public async Task<ArticleLocation> UpdateArticleLocation(UpdateArticleLocationRequest request)
    {
        if (request.Count < 0)
        {
            throw new InvalidValue(ExceptionMessages.INVALID_ARTICLE_COUNT);
        }
        
        GetArticleLocationRequest getRequest = new GetArticleLocationRequest
        {
            ArticleCode = request.ArticleCode,
            LocationCode = request.LocationCode
        };
        
        if (await _articleLocationsRepository.GetAsync(getRequest) == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST);
        }

        ArticleLocation result = await _articleLocationsRepository.UpdateAsync(request);
        return result;
    }

    public async Task<ArticleLocation> DeleteArticleLocation(DeleteArticleLocationRequest request)
    {
        GetArticleLocationRequest getRequest = new GetArticleLocationRequest
        {
            ArticleCode = request.ArticleCode,
            LocationCode = request.LocationCode
        };
        
        if (await _articleLocationsRepository.GetAsync(getRequest) == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST);
        }

        ArticleLocation result = await _articleLocationsRepository.DeleteAsync(request);
        return result;
    }
}