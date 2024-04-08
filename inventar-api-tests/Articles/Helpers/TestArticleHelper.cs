using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;

namespace inventar_api_tests.Articles.Helpers;

public static class TestArticleHelper
{
    public static List<Article> CreateArticles(int count)
    {
        List<Article> list = new List<Article>();

        for (int i = 1; i <= count; i++)
        {
            list.Add(CreateArticle(i));
        }

        return list;
    }
    
    public static Article CreateArticle(int id)
    {
        return new Article
        {
            Id = id,
            Code = id,
            Name = $"Article {id}",
            Consumption = "",
            Machinery = ""
        };
    }
    
    public static CreateArticleRequest CreateCreateArticleRequest(int id)
    {
        return new CreateArticleRequest
        {
            Code = id,
            Name = $"Article {id}",
            Consumption = "",
            Machinery = ""
        };
    }
    
    public static UpdateArticleRequest CreateUpdateArticleRequest(int id)
    {
        return new UpdateArticleRequest
        {
            Code = id,
            Name = $"Article {id}",
            Consumption = "",
            Machinery = ""
        };
    }
}