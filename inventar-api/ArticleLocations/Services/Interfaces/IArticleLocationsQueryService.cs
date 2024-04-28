using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;

namespace inventar_api.ArticleLocations.Services.Interfaces;

public interface IArticleLocationsQueryService
{
    Task<IEnumerable<ArticleLocation>> GetAllArticleLocations();
    Task<IEnumerable<ArticleLocationHistory>> GetStockHistory();
    Task<ArticleLocation> GetArticleLocation(GetArticleLocationRequest request);
}