using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;

namespace inventar_api_tests.ArticleLocations.Helpers;

public static class TestArticleLocationHelper
{
    public static List<ArticleLocation> CreateArticleLocations(int count)
    {
        List<ArticleLocation> list = new List<ArticleLocation>();

        for (int i = 1; i <= count; i++)
        {
            list.Add(CreateArticleLocation(i));
        }

        return list;
    }
    
    public static ArticleLocation CreateArticleLocation(int id)
    {
        return new ArticleLocation
        {
            Id = id,
            ArticleId = id,
            LocationId = id,
            Count = 10
        };
    }

    public static CreateArticleLocationRequest CreateCreateArticleLocationRequest(int id)
    {
        return new CreateArticleLocationRequest
        {
            ArticleId = id,
            LocationId = id,
            Count = 10
        };
    }
    
    public static UpdateArticleLocationRequest CreateUpdateArticleLocationRequest(int id)
    {
        return new UpdateArticleLocationRequest
        {
            ArticleId = id,
            LocationId = id,
            Count = 10
        };
    }
    
    public static DeleteArticleLocationRequest CreateDeleteArticleLocationRequest(int id)
    {
        return new DeleteArticleLocationRequest
        {
            ArticleCode = id,
            LocationCode = $"{id}"
        };
    }
    
    public static GetArticleLocationRequest CreateGetArticleLocationRequest(int id)
    {
        return new GetArticleLocationRequest
        {
            ArticleCode = id,
            LocationCode = $"{id}"
        };
    }
}