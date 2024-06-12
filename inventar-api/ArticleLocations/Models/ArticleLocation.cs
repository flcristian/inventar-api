using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using inventar_api.Articles.Models;
using inventar_api.Locations.Models;

namespace inventar_api.ArticleLocations.Models;

[Table("articlelocations")]
public class ArticleLocation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
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
    [Column("count")]
    public int Count { get; set; }
}