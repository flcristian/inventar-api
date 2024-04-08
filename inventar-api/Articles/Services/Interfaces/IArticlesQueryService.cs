using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;

namespace inventar_api.Articles.Services.Interfaces;

public interface IArticlesQueryService
{
    Task<IEnumerable<Article>> GetAllArticles();
    Task<Article> GetArticleByCode(int code);
}