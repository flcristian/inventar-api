namespace inventar_api.ArticleLocations.DTOs;

public class DeleteStockHistoryRequest
{
    public int ArticleCode { get; set; }
    public string LocationCode { get; set; }
}