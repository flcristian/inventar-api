using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;

namespace inventar_api.Articles.Repository.Interfaces;

public interface IArticlesRepository
{
    Task<IEnumerable<Article>> GetAllAsync();
    Task<Article?> GetByCodeAsync(int code);
    Task<Article> CreateAsync(CreateArticleRequest request);
    Task<Article> UpdateAsync(UpdateArticleRequest request);
    Task<Article> DeleteAsync(int code);
}