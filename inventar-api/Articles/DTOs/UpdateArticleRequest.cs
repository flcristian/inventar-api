namespace inventar_api.Articles.DTOs;

public class UpdateArticleRequest
{
    public int Code { get; set; }
    
    public string Name { get; set; }
    
    public string Consumption { get; set; }
    
    public string Machinery { get; set; }
}