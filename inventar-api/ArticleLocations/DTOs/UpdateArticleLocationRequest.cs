namespace inventar_api.ArticleLocations.DTOs;

public class UpdateArticleLocationRequest
{
    public int ArticleCode { get; set; }

    public string LocationCode { get; set; }
    
    public int Count { get; set; }
}