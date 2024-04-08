using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.ArticleLocations.Repository.Interfaces;
using inventar_api.ArticleLocations.Services.Interfaces;
using inventar_api.Articles.Repository.Interfaces;
using inventar_api.Locations.Repository.Interfaces;

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
        throw new NotImplementedException();
    }

    public async Task<ArticleLocation> UpdateArticleLocation(UpdateArticleLocationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ArticleLocation> DeleteArticleLocation(DeleteArticleLocationRequest request)
    {
        throw new NotImplementedException();
    }
}