namespace inventar_api.ArticleLocations.DTOs;

public class CreateArticleLocationRequest
{
    public int ArticleId { get; set; }

    public int LocationId { get; set; }
    
    public int Count { get; set; }
}