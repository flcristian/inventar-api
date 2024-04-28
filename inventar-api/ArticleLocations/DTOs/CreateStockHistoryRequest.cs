namespace inventar_api.ArticleLocations.DTOs;

public class CreateStockHistoryRequest
{
    public DateTime Date { get; set; }
    public int ArticleCode { get; set; }
    public string LocationCode { get; set; }
    public int StockIn { get; set; }
    public int StockOut { get; set; }
    public int Order { get; set; }
    public int Necessary { get; set; }
    public string Source { get; set; }
}