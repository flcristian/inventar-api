using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;

namespace inventar_api.ArticleLocations.Repository.Interfaces;

public interface IArticleLocationsRepository
{
    Task<IEnumerable<ArticleLocation>> GetAllAsync();
    Task<ArticleLocation?> GetAsync(GetArticleLocationRequest request);
    Task<ArticleLocation> CreateAsync(CreateArticleLocationRequest request);
    Task<ArticleLocation> UpdateAsync(UpdateArticleLocationRequest request);
    Task<ArticleLocation> DeleteAsync(DeleteArticleLocationRequest request);
}