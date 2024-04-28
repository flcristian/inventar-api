using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;

namespace inventar_api.ArticleLocations.Repository.Interfaces;

public interface IArticleLocationsRepository
{
    Task<IEnumerable<ArticleLocation>> GetAllAsync();
    Task<IEnumerable<ArticleLocationHistory>> GetHistoryAsync();
    Task<ArticleLocation?> GetAsync(GetArticleLocationRequest request);
    Task<ArticleLocationHistory?> GetHistoryByIdAsync(int id);
    Task<ArticleLocation> CreateAsync(CreateArticleLocationRequest request);
    Task<ArticleLocationHistory> CreateHistoryAsync(CreateStockHistoryRequest request);
    Task<ArticleLocation> UpdateAsync(UpdateArticleLocationRequest request);
    Task<ArticleLocation> DeleteAsync(DeleteArticleLocationRequest request);
    Task<ArticleLocationHistory> DeleteHistoryAsync(int id);
}