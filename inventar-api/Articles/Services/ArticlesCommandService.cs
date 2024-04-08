using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;
using inventar_api.Articles.Repository.Interfaces;
using inventar_api.Articles.Services.Interfaces;
using inventar_api.System.Constants;
using inventar_api.System.Exceptions;

namespace inventar_api.Articles.Services;

public class ArticlesCommandService : IArticlesCommandService
{
    private IArticlesRepository _repository;

    public ArticlesCommandService(IArticlesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Article> CreateArticle(CreateArticleRequest request)
    {
        if (request.Code < 0)
        {
            throw new InvalidValue(ExceptionMessages.INVALID_ARTICLE_CODE);
        }

        if (await _repository.GetByCodeAsync(request.Code) != null)
        {
            throw new ItemAlreadyExists(ExceptionMessages.ARTICLE_ALREADY_EXISTS);
        }

        return await _repository.CreateAsync(request);
    }

    public async Task<Article> UpdateArticle(UpdateArticleRequest request)
    {
        if (request.Code < 0)
        {
            throw new InvalidValue(ExceptionMessages.INVALID_ARTICLE_CODE);
        }
        
        if (await _repository.GetByCodeAsync(request.Code) == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.ARTICLE_DOES_NOT_EXIST);
        }

        return await _repository.UpdateAsync(request);
    }

    public async Task<Article> DeleteArticle(int code)
    {
        if (await _repository.GetByCodeAsync(code) == null)
        {
            throw new ItemDoesNotExist(ExceptionMessages.ARTICLE_DOES_NOT_EXIST);
        }

        return await _repository.DeleteAsync(code);
    }
}