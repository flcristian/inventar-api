using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using inventar_api.Articles.Models;
using inventar_api.Locations.Models;

namespace inventar_api.ArticleLocations.Models;

public class ArticleLocationHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("date")]
    public DateTime Date { get; set; }
    
    [Required]
    [Column("articleCode")]
    public int ArticleCode { get; set; }
    
    [JsonIgnore]
    public virtual Article Article { get; set; }
    
    [Required]
    [Column("locationCode")]
    public string LocationCode { get; set; }
    
    [JsonIgnore]
    public virtual Location Location { get; set; }
    
    [Required]
    [Column("stockIn")]
    public int StockIn { get; set; }
    
    [Required]
    [Column("stockOut")]
    public int StockOut { get; set; }
    
    [Required]
    [Column("order")]
    public int Order { get; set; }
    
    [Required]
    [Column("necessary")]
    public int Necessary { get; set; }
    
    [Required]
    [Column("source")]
    public string Source { get; set; }
}