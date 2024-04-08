namespace inventar_api.ArticleLocations.DTOs;

public class DeleteArticleLocationRequest
{
    public int ArticleCode { get; set; }

    public string LocationCode { get; set; }
}