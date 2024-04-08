using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;
using inventar_api.Articles.Repository.Interfaces;
using inventar_api.Articles.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;

namespace inventar_api.Articles.Services;

public class ArticlesQueryService : IArticlesQueryService
{
    private IArticlesRepository _repository;

    public ArticlesQueryService(IArticlesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Article>> GetAllArticles()
    {
        IEnumerable<Article> result = await _repository.GetAllAsync();

        if (result.Count() == 0)
        {
            throw new ItemsDoNotExist(ExceptionMessages.ARTICLES_DO_NOT_EXIST);
        }

        return result;
    }

    public async Task<Article> GetArticleByCode(int code)
    {
        Article? result = await _repository.GetByCodeAsync(code);

        if (result == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.ARTICLE_DOES_NOT_EXIST);
        }

        return result;
    }
}