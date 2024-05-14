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

    public async Task<ArticleLocationHistory> CreateStockHistory(CreateStockHistoryRequest request)
    {
        if (request.StockIn < 0 || request.StockOut < 0 || request.Order < 0 || request.Necessary < 0)
        {
            throw new InvalidValue(ExceptionMessages.INVALID_STOCK_COUNT);
        }
        
        if (await _articlesRepository.GetByCodeAsync(request.ArticleCode) == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.ARTICLE_DOES_NOT_EXIST);
        }
        
        if (await _locationsRepository.GetByCodeAsync(request.LocationCode) == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.LOCATION_DOES_NOT_EXIST);
        }

        ArticleLocation? al = await _articleLocationsRepository.GetAsync(new GetArticleLocationRequest
            { ArticleCode = request.ArticleCode, LocationCode = request.LocationCode });
        
        if (al == null)
        {
            CreateArticleLocationRequest createRequest = new CreateArticleLocationRequest
            {
                ArticleCode = request.ArticleCode, LocationCode = request.LocationCode,
                Count = request.StockIn - request.StockOut
            };
            await _articleLocationsRepository.CreateAsync(createRequest);
        }
        else
        {
            UpdateArticleLocationRequest updateRequest = new UpdateArticleLocationRequest
            {
                ArticleCode = request.ArticleCode, LocationCode = request.LocationCode,
                Count = al.Count + request.StockIn - request.StockOut
            };
            await _articleLocationsRepository.UpdateAsync(updateRequest);
        }

        ArticleLocationHistory result = await _articleLocationsRepository.CreateHistoryAsync(request);
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

    public async Task<ArticleLocationHistory> DeleteStockHistory(int id)
    {
        ArticleLocationHistory? history = await _articleLocationsRepository.GetHistoryByIdAsync(id);
        if (history == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.ARTICLE_LOCATION_DOES_NOT_EXIST);
        }
        
        ArticleLocation al = (await _articleLocationsRepository.GetAsync(new GetArticleLocationRequest
            { ArticleCode = history.ArticleCode, LocationCode = history.LocationCode }))!;
        
        UpdateArticleLocationRequest updateRequest = new UpdateArticleLocationRequest
        {
            ArticleCode = history.ArticleCode, LocationCode = history.LocationCode,
            Count = al.Count - history.StockIn + history.StockOut
        };
        
        if (updateRequest.Count == 0)
        {
            await _articleLocationsRepository.DeleteAsync(new DeleteArticleLocationRequest
            {
                ArticleCode = history.ArticleCode, LocationCode = history.LocationCode
            });
        }
        else
        {
            await _articleLocationsRepository.UpdateAsync(updateRequest);
        }

        ArticleLocationHistory result = await _articleLocationsRepository.DeleteHistoryAsync(id);
        return result;
    }
}