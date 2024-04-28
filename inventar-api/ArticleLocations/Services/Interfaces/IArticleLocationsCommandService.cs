using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;

namespace inventar_api.ArticleLocations.Services.Interfaces;

public interface IArticleLocationsCommandService
{
    Task<ArticleLocation> CreateArticleLocation(CreateArticleLocationRequest request);
    Task<ArticleLocationHistory> CreateStockHistory(CreateStockHistoryRequest request);
    Task<ArticleLocation> UpdateArticleLocation(UpdateArticleLocationRequest request);
    Task<ArticleLocation> DeleteArticleLocation(DeleteArticleLocationRequest request);
    Task<ArticleLocationHistory> DeleteStockHistory(int id);
}