namespace inventar_api.ArticleLocations.DTOs;

public class GetArticleLocationRequest
{
    public int ArticleCode { get; set; }

    public string LocationCode { get; set; }
}