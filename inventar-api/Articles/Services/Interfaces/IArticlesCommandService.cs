using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;

namespace inventar_api.Articles.Services.Interfaces;

public interface IArticlesCommandService
{
    Task<Article> CreateArticle(CreateArticleRequest request);
    Task<Article> UpdateArticle(UpdateArticleRequest request);
    Task<Article> DeleteArticle(int code);
}