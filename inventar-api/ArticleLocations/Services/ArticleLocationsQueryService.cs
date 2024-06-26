using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.ArticleLocations.Repository.Interfaces;
using inventar_api.ArticleLocations.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;

namespace inventar_api.ArticleLocations.Services;

public class ArticleLocationsQueryService : IArticleLocationsQueryService
{
    private IArticleLocationsRepository _repository;

    public ArticleLocationsQueryService(IArticleLocationsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ArticleLocation>> GetAllArticleLocations()
    {
        IEnumerable<ArticleLocation> result = await _repository.GetAllAsync();
        
        return result;
    }

    public async Task<IEnumerable<ArticleLocationHistory>> GetStockHistory()
    {
        IEnumerable<ArticleLocationHistory> result = await _repository.GetHistoryAsync();

        return result;
    }

    public async Task<ArticleLocation> GetArticleLocation(GetArticleLocationRequest request)
    {
        ArticleLocation? result = await _repository.GetAsync(request);

        if (result == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST);
        }

        return result;
    }
}